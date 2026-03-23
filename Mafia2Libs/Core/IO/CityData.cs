using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.IO
{
    public class CityData
    {
        public List<PointData> Points { get; set; }
        public List<PolygonData> Polygons { get; set; }

        public CityData()
        {
            Points = new List<PointData>();
            Polygons = new List<PolygonData>();
        }

        public static CityData Load(string filename)
        {
            var data = new CityData();
            XDocument doc = XDocument.Load(filename);
            XElement root = doc.Root;

            XElement pointsElem = root.Element("Points");
            foreach (var pointElem in pointsElem.Elements())
            {
                float x = float.Parse(pointElem.Element("x").Value, CultureInfo.InvariantCulture);
                float y = float.Parse(pointElem.Element("y").Value, CultureInfo.InvariantCulture);
                data.Points.Add(new PointData(x, y));
            }

            XElement polygonsElem = root.Element("Polygons");
            foreach (var polyElem in polygonsElem.Elements())
            {
                string name = polyElem.Element("Name").Value;
                string textId = polyElem.Element("TextID").Value;
                List<int> indices = new List<int>();
                XElement pointsRefs = polyElem.Element("Points");
                foreach (var idxElem in pointsRefs.Elements())
                {
                    int idx = int.Parse(idxElem.Value);
                    indices.Add(idx);
                }
                data.Polygons.Add(new PolygonData(name, textId, indices));
            }

            return data;
        }

        public void Save(string filename)
        {
            XDocument doc = new XDocument();
            XElement root = new XElement("Root");

            XElement pointsElem = new XElement("Points");
            for (int i = 0; i < Points.Count; i++)
            {
                XElement item = new XElement($"Item{i}");
                XElement xElem = new XElement("x", Points[i].X.ToString(CultureInfo.InvariantCulture));
                XElement yElem = new XElement("y", Points[i].Y.ToString(CultureInfo.InvariantCulture));
                xElem.SetAttributeValue("__type", "x");
                yElem.SetAttributeValue("__type", "x");
                item.Add(xElem, yElem);
                pointsElem.Add(item);
            }
            root.Add(pointsElem);

            XElement polygonsElem = new XElement("Polygons");
            for (int i = 0; i < Polygons.Count; i++)
            {
                XElement item = new XElement($"Item{i}");
                XElement nameElem = new XElement("Name", Polygons[i].Name);
                nameElem.SetAttributeValue("__type", "x");
                XElement textIdElem = new XElement("TextID", Polygons[i].TextID);
                textIdElem.SetAttributeValue("__type", "x");
                XElement pointsRefs = new XElement("Points");
                for (int j = 0; j < Polygons[i].PointIndices.Count; j++)
                {
                    XElement idxElem = new XElement($"Item{j}", Polygons[i].PointIndices[j]);
                    idxElem.SetAttributeValue("__type", "x");
                    pointsRefs.Add(idxElem);
                }
                item.Add(nameElem, textIdElem, pointsRefs);
                polygonsElem.Add(item);
            }
            root.Add(polygonsElem);

            doc.Add(root);
            doc.Save(filename);
        }
    }

    public class PointData
    {
        public float X { get; set; }
        public float Y { get; set; }

        public PointData(float x, float y)
        {
            X = x;
            Y = y;
        }
    }

    public class PolygonData
    {
        public string Name { get; set; }
        public string TextID { get; set; }
        public List<int> PointIndices { get; set; }

        public PolygonData(string name, string textId, List<int> indices)
        {
            Name = name;
            TextID = textId;
            PointIndices = indices;
        }
    }
}
