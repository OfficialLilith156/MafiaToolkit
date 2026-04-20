using ResourceTypes.Prefab.Vehicle;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Utils.Helpers.Reflection
{
    public class ReflectionHelpers
    {
        public static void Copy<T>(T FromObject, ref T ToObject)
        {
            Type ObjectType = FromObject.GetType();
            if (ObjectType.IsPrimitive)
            {
                ToObject = FromObject;
            }
            else
            {
                foreach (PropertyInfo Info in ObjectType.GetProperties())
                {
                    if (Info.PropertyType.IsArray)
                    {
                        Array FromObjectArray = (Array)Info.GetValue(FromObject);
                        if (FromObjectArray == null)
                        {
                            if (Info.CanWrite) Info.SetValue(ToObject, null);
                            continue;
                        }

                        Array ArrayObject = Array.CreateInstance(Info.PropertyType.GetElementType(), FromObjectArray.Length);
                        for (int i = 0; i < ArrayObject.Length; i++)
                        {
                            object FromItem = FromObjectArray.GetValue(i);
                            if (FromItem == null)
                            {
                                ArrayObject.SetValue(null, i);
                                continue;
                            }
                            object ToItem = Activator.CreateInstance(FromItem.GetType());
                            Copy(FromItem, ref ToItem);
                            ArrayObject.SetValue(ToItem, i);
                        }
                        if (Info.CanWrite) Info.SetValue(ToObject, ArrayObject);
                    }
                    else if (Info.PropertyType.IsClass)
                    {
                        object FromItem = Info.GetValue(FromObject);
                        if (FromItem == null)
                        {
                            if (Info.CanWrite) Info.SetValue(ToObject, null);
                            continue;
                        }
                        Type FromType = FromItem.GetType();
                        if (FromType.GetConstructor(Type.EmptyTypes) != null)
                        {
                            object ToItem = Activator.CreateInstance(FromItem.GetType());
                            Copy(FromItem, ref ToItem);
                            if (Info.CanWrite) Info.SetValue(ToObject, ToItem);
                            continue;
                        }
                        if (Info.CanWrite)
                        {
                            Info.SetValue(ToObject, Info.GetValue(FromObject));
                        }
                    }
                    else if (Info.CanWrite)
                    {
                        Info.SetValue(ToObject, Info.GetValue(FromObject));
                    }
                    else
                    {
                        object ToCopy = Info.GetValue(FromObject);
                        object NewObject = Info.GetValue(ToObject);
                        if (NewObject == null || ToCopy == null) continue;
                        FieldInfo[] Fields = Info.PropertyType.GetFields();
                        for (int i = 0; i < Fields.Length; i++)
                        {
                            FieldInfo ThisField = Fields[i];
                            if (!ThisField.Attributes.HasFlag(FieldAttributes.Static))
                            {
                                ThisField.SetValue(NewObject, ThisField.GetValue(ToCopy));
                            }
                        }
                    }
                }
            }
        }

        public static T ConvertToPropertyFromXML<T>(XElement Node)
        {
            Type XMLType = GetTypeByName(Node.Name.LocalName);
            if (XMLType == null)
                throw new InvalidOperationException($"Type {Node.Name.LocalName} not found");
            T TypedObject = (T)Activator.CreateInstance(XMLType);

            PropertyInfo[] Properties = TypedObject.GetType().GetProperties();
            foreach (PropertyInfo Info in Properties)
            {
                if (!AllowPropertyToReflect(Info)) continue;
                bool bForceAsAttribute = ForcePropertyAsAttribute(Info);

                if (Info.PropertyType.IsArray)
                {
                    XElement Element = Node.Element(Info.Name);
                    if (Element == null) continue;
                    int count = Element.Elements().Count();
                    Array ArrayObject = Array.CreateInstance(Info.PropertyType.GetElementType(), count);
                    for (int i = 0; i < count; i++)
                    {
                        object ElementObject = InternalConvertProperty(Element.Elements().ElementAt(i), Info.PropertyType.GetElementType());
                        ArrayObject.SetValue(ElementObject, i);
                    }
                    PropertyInfo targetProp = TypedObject.GetType().GetProperty(Info.Name);
                    if (targetProp != null && targetProp.CanWrite)
                        targetProp.SetValue(TypedObject, ArrayObject);
                    continue;
                }
                else if (Info.PropertyType.IsClass && AllowClassReflection(Info.PropertyType))
                {
                    XElement Element = Node.Element(Info.Name);
                    if (Element == null) continue;
                    object ClassObject = InternalConvertProperty(Element, Info.PropertyType);
                    if (Info.CanWrite) Info.SetValue(TypedObject, ClassObject);
                    continue;
                }
                else if (Info.PropertyType.IsClass)
                {
                    XElement Element = Node.Element(Info.Name);
                    if (Element == null) continue;
                    object ClassObject = InternalConvertProperty(Element, Info.PropertyType);
                    if (Info.CanWrite)
                        Info.SetValue(TypedObject, Convert.ChangeType(ClassObject, Info.PropertyType));
                    continue;
                }
                else
                {
                    string NodeContent = null;
                    if (bForceAsAttribute)
                    {
                        XAttribute attr = Node.Attribute(Info.Name);
                        if (attr == null) continue;
                        NodeContent = attr.Value;
                    }
                    else
                    {
                        XElement elem = Node.Element(Info.Name);
                        if (elem == null) continue;
                        NodeContent = elem.Value;
                    }

                    if (!string.IsNullOrEmpty(NodeContent) && Info.CanWrite)
                    {
                        if (Info.PropertyType.IsEnum)
                        {
                            object Value = Enum.Parse(Info.PropertyType, NodeContent);
                            Info.SetValue(TypedObject, Value);
                            continue;
                        }
                        else if (Info.PropertyType == typeof(float))
                        {
                            Info.SetValue(TypedObject, ToSingle(NodeContent));
                        }
                        else if (Info.PropertyType == typeof(double))
                        {
                            Info.SetValue(TypedObject, ToDouble(NodeContent));
                        }
                        else
                        {
                            var props = TypeDescriptor.GetProperties(TypedObject);
                            var converter = props[Info.Name].Converter;
                            if (converter.CanConvertFrom(NodeContent.GetType()))
                            {
                                Info.SetValue(TypedObject, converter.ConvertFromInvariantString(NodeContent));
                            }
                            else
                            {
                                Info.SetValue(TypedObject, Convert.ChangeType(NodeContent, Info.PropertyType));
                            }
                        }
                    }
                }
            }
            return TypedObject;
        }

        private static object InternalConvertProperty(XElement Node, Type ElementType)
        {
            if (ElementType == typeof(string)) return Node.Value;
            if (ElementType == typeof(float)) return ToSingle(Node.Value);
            if (ElementType == typeof(double)) return ToDouble(Node.Value);

            if (ElementType.IsInterface)
            {
                string NameSpace = ElementType.Namespace;
                string Name = Node.Name.LocalName;
                Type Test = Type.GetType(NameSpace + "." + Name, false);
                if (Test != null) ElementType = Test;
            }
            else if (ElementType.IsClass && CheckForDerivedClass(ElementType))
            {
                XAttribute TypeAttribute = Node.Attribute("Type");
                if (TypeAttribute != null)
                {
                    string Name = TypeAttribute.Value;
                    Type Test = GetTypeByName(Name);
                    if (Test != null && Test.IsAssignableTo(ElementType))
                        ElementType = Test;
                }
            }

            object TypedObject = Activator.CreateInstance(ElementType);
            if (ElementType.GetProperties().Length == 0)
            {
                return Convert.ChangeType(Node.Value, ElementType);
            }

            foreach (PropertyInfo Info in ElementType.GetProperties())
            {
                if (!AllowPropertyToReflect(Info)) continue;
                bool bForceAsAttribute = ForcePropertyAsAttribute(Info);

                if (Info.PropertyType.IsClass && AllowClassReflection(Info.PropertyType))
                {
                    XElement Element = Node.Element(Info.Name);
                    if (Element == null) continue;
                    object ClassObject = InternalConvertProperty(Element, Info.PropertyType);
                    if (Info.CanWrite) Info.SetValue(TypedObject, ClassObject);
                    continue;
                }
                else if (Info.PropertyType.IsArray)
                {
                    XElement Element = Node.Element(Info.Name);
                    if (Element == null) continue;
                    int count = Element.Elements().Count();
                    Array ArrayObject = Array.CreateInstance(Info.PropertyType.GetElementType(), count);
                    for (int i = 0; i < count; i++)
                    {
                        object ElementObject = InternalConvertProperty(Element.Elements().ElementAt(i), Info.PropertyType.GetElementType());
                        ArrayObject.SetValue(ElementObject, i);
                    }
                    PropertyInfo targetProp = ElementType.GetProperty(Info.Name);
                    if (targetProp != null && targetProp.CanWrite)
                        targetProp.SetValue(TypedObject, ArrayObject);
                    continue;
                }

                string NodeContent = null;
                if (bForceAsAttribute)
                {
                    XAttribute attr = Node.Attribute(Info.Name);
                    if (attr == null) continue;
                    NodeContent = attr.Value;
                }
                else
                {
                    XElement elem = Node.Element(Info.Name);
                    if (elem == null) continue;
                    NodeContent = elem.Value;
                }

                if (!string.IsNullOrEmpty(NodeContent) && Info.CanWrite)
                {
                    if (Info.PropertyType.IsEnum)
                    {
                        object Value = Enum.Parse(Info.PropertyType, NodeContent);
                        Info.SetValue(TypedObject, Value);
                        continue;
                    }
                    else if (Info.PropertyType == typeof(float))
                    {
                        Info.SetValue(TypedObject, ToSingle(NodeContent));
                    }
                    else if (Info.PropertyType == typeof(double))
                    {
                        Info.SetValue(TypedObject, ToDouble(NodeContent));
                    }
                    else if (Info.PropertyType.IsClass && AllowClassReflection(Info.PropertyType))
                    {
                        XElement Element = Node.Element(Info.Name);
                        if (Element == null) continue;
                        object ClassObject = InternalConvertProperty(Element, Info.PropertyType);
                        if (Info.CanWrite) Info.SetValue(TypedObject, ClassObject);
                    }
                    else
                    {
                        var props = TypeDescriptor.GetProperties(TypedObject);
                        var converter = props[Info.Name].Converter;
                        if (converter.CanConvertFrom(NodeContent.GetType()))
                        {
                            Info.SetValue(TypedObject, converter.ConvertFromInvariantString(NodeContent));
                        }
                        else
                        {
                            Info.SetValue(TypedObject, Convert.ChangeType(NodeContent, Info.PropertyType));
                        }
                    }
                }
            }
            return TypedObject;
        }

        private static XElement InternalConvertProperty<TObject>(TObject PropertyData, Type ObjectType, string PropertyName)
        {
            if (ObjectType.IsArray)
            {
                XElement RootElement = new XElement("Root");
                Array ArrayContent = (Array)Convert.ChangeType(PropertyData, ObjectType);
                foreach (object Element in ArrayContent)
                {
                    XElement Entry = ConvertPropertyToXML(Element);
                    RootElement.Add(Entry);
                }
                return RootElement;
            }
            else if (AllowClassReflection(ObjectType))
            {
                XElement Element = new XElement(PropertyName, new XAttribute("Type", ObjectType.Name));
                ConvertObject(Element, PropertyData, ObjectType);
                return Element;
            }
            else
            {
                XElement Element = new XElement(ObjectType.Name);
                ConvertObject(Element, PropertyData, ObjectType);
                return Element;
            }
        }

        private static void ConvertObject<TObject>(XElement Element, TObject PropertyData, Type ObjectType)
        {
            if (ObjectType.GetProperties().Length == 0)
            {
                Element.SetValue(PropertyData);
                return;
            }

            foreach (PropertyInfo Info in ObjectType.GetProperties())
            {
                if (!AllowPropertyToReflect(Info)) continue;
                bool bForceAsAttribute = ForcePropertyAsAttribute(Info);

                if (Info.PropertyType.IsArray)
                {
                    XElement RootElement = new XElement(Info.Name);
                    Array ArrayContent = (Array)PropertyData.GetType().GetProperty(Info.Name).GetValue(PropertyData);
                    if (ArrayContent != null)
                    {
                        foreach (object ArrayElement in ArrayContent)
                        {
                            XElement Entry = ConvertPropertyToXML(ArrayElement);
                            RootElement.Add(Entry);
                        }
                    }
                    Element.Add(RootElement);
                }
                else if (Info.PropertyType.IsClass && AllowClassReflection(Info.PropertyType))
                {
                    object ClassObject = PropertyData.GetType().GetProperty(Info.Name).GetValue(PropertyData);
                    if (ClassObject != null)
                        Element.Add(InternalConvertProperty(ClassObject, ClassObject.GetType(), Info.Name));
                }
                else
                {
                    var props = TypeDescriptor.GetProperties(PropertyData);
                    var converter = props[Info.Name].Converter;
                    object info = PropertyData.GetType().GetProperty(Info.Name).GetValue(PropertyData);
                    info = info ?? "";
                    info = converter.ConvertToString(info);
                    if (bForceAsAttribute)
                    {
                        Element.Add(new XAttribute(Info.Name, info));
                    }
                    else
                    {
                        Element.Add(new XElement(Info.Name, new XAttribute("Type", Info.PropertyType.Name), info));
                    }
                }
            }
        }

        public static XElement ConvertPropertyToXML<TObject>(TObject PropertyData)
        {
            return InternalConvertProperty(PropertyData, PropertyData.GetType(), "Element");
        }

        private static bool ForcePropertyAsAttribute(PropertyInfo Info)
        {
            Attribute PropertyAttritbute = Info.GetCustomAttribute(typeof(PropertyForceAsAttributeAttribute));
            if (PropertyAttritbute != null)
            {
                return true;
            }
            return false;
        }

        private static bool AllowPropertyToReflect(PropertyInfo Info)
        {
            Attribute PropertyAttritbute = Info.GetCustomAttribute(typeof(PropertyIgnoreByReflector));
            return PropertyAttritbute == null;
        }

        private static bool AllowClassReflection(Type Info)
        {
            Attribute PropertyAttritbute = Info.GetCustomAttribute(typeof(PropertyClassAllowReflection));
            return PropertyAttritbute != null;
        }

        public static bool CheckForDerivedClass(Type Info)
        {
            Attribute PropertyAttritbute = Info.GetCustomAttribute(typeof(PropertyClassCheckInherited));
            return PropertyAttritbute != null;
        }

        private static Type GetTypeByName(string Name)
        {
            Assembly OurAssembly = Assembly.GetExecutingAssembly();
            foreach (TypeInfo DefinedType in OurAssembly.DefinedTypes)
            {
                if (DefinedType.Name.Equals(Name))
                    return DefinedType;
            }
            return null;
        }

        internal static readonly char[] WhitespaceChars = new char[] { ' ', '\t', '\n', '\r' };
        internal static string TrimString(string value) => value.Trim(WhitespaceChars);

        private static float ToSingle(string s)
        {
            s = TrimString(s);
            s = s.Replace(',', '.');
            if (s == "-INF") return Single.NegativeInfinity;
            if (s == "INF") return Single.PositiveInfinity;
            float f = float.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
            if (f == 0 && s[0] == '-') return -0f;
            return f;
        }

        private static double ToDouble(string s)
        {
            s = TrimString(s);
            s = s.Replace(',', '.');
            if (s == "-INF") return Double.NegativeInfinity;
            if (s == "INF") return Double.PositiveInfinity;
            double dVal = double.Parse(s, NumberStyles.Any, CultureInfo.InvariantCulture);
            if (dVal == 0 && s[0] == '-') return -0d;
            return dVal;
        }
    }
}