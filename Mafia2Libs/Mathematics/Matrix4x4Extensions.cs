using System;
using System.Numerics;

namespace Toolkit.Mathematics
{
    public static class Matrix4x4Extensions
    {
        public static Vector3 GetTranslation(this Matrix4x4 m)
        {
            return new Vector3(m.M41, m.M42, m.M43);
        }

        public static Matrix4x4 SetTranslation(this Matrix4x4 m, Vector3 translation)
        {
            // Создаем новую матрицу с обновленным переводом
            Matrix4x4 result = m;
            result.M41 = translation.X;
            result.M42 = translation.Y;
            result.M43 = translation.Z;
            return result;
        }

        public static Matrix4x4 SetTranslation(this Matrix4x4 m, float x, float y, float z)
        {
            return m.SetTranslation(new Vector3(x, y, z));
        }

        public static Quaternion GetRotation(this Matrix4x4 m)
        {
            float m11 = m.M11, m12 = m.M12, m13 = m.M13;
            float m21 = m.M21, m22 = m.M22, m23 = m.M23;
            float m31 = m.M31, m32 = m.M32, m33 = m.M33;
            float trace = m11 + m22 + m33;
            Quaternion q = new();
            if (trace > 0)
            {
                float s = (float)Math.Sqrt(trace + 1.0f) * 2.0f;
                q.W = 0.25f * s;
                q.X = (m32 - m23) / s;
                q.Y = (m13 - m31) / s;
                q.Z = (m21 - m12) / s;
            }
            else if (m11 > m22 && m11 > m33)
            {
                float s = (float)Math.Sqrt(1.0f + m11 - m22 - m33) * 2.0f;
                q.W = (m32 - m23) / s;
                q.X = 0.25f * s;
                q.Y = (m12 + m21) / s;
                q.Z = (m13 + m31) / s;
            }
            else if (m22 > m33)
            {
                float s = (float)Math.Sqrt(1.0f + m22 - m11 - m33) * 2.0f;
                q.W = (m13 - m31) / s;
                q.X = (m12 + m21) / s;
                q.Y = 0.25f * s;
                q.Z = (m23 + m32) / s;
            }
            else
            {
                float s = (float)Math.Sqrt(1.0f + m33 - m11 - m22) * 2.0f;
                q.W = (m21 - m12) / s;
                q.X = (m13 + m31) / s;
                q.Y = (m23 + m32) / s;
                q.Z = 0.25f * s;
            }
            return q;
        }

        // Дополнительные полезные методы для работы с матрицами:

        public static Vector3 GetScale(this Matrix4x4 m)
        {
            // Масштаб - это длина базисных векторов
            return new Vector3(
                new Vector3(m.M11, m.M12, m.M13).Length(),
                new Vector3(m.M21, m.M22, m.M23).Length(),
                new Vector3(m.M31, m.M32, m.M33).Length()
            );
        }

        public static Matrix4x4 SetScale(this Matrix4x4 m, Vector3 scale)
        {
            // Сохраняем поворот и перемещение, меняем только масштаб
            Matrix4x4 result = m;

            // Нормализуем векторы и умножаем на новый масштаб
            Vector3 xAxis = new Vector3(m.M11, m.M12, m.M13);
            Vector3 yAxis = new Vector3(m.M21, m.M22, m.M23);
            Vector3 zAxis = new Vector3(m.M31, m.M32, m.M33);

            if (xAxis.LengthSquared() > 0) xAxis = Vector3.Normalize(xAxis) * scale.X;
            if (yAxis.LengthSquared() > 0) yAxis = Vector3.Normalize(yAxis) * scale.Y;
            if (zAxis.LengthSquared() > 0) zAxis = Vector3.Normalize(zAxis) * scale.Z;

            result.M11 = xAxis.X; result.M12 = xAxis.Y; result.M13 = xAxis.Z;
            result.M21 = yAxis.X; result.M22 = yAxis.Y; result.M23 = yAxis.Z;
            result.M31 = zAxis.X; result.M32 = zAxis.Y; result.M33 = zAxis.Z;

            return result;
        }

        public static Matrix4x4 CreateFromTRS(Vector3 translation, Quaternion rotation, Vector3 scale)
        {
            // Создаем матрицу поворота
            Matrix4x4 result = Matrix4x4.CreateFromQuaternion(rotation);

            // Применяем масштаб
            result.M11 *= scale.X;
            result.M12 *= scale.X;
            result.M13 *= scale.X;

            result.M21 *= scale.Y;
            result.M22 *= scale.Y;
            result.M23 *= scale.Y;

            result.M31 *= scale.Z;
            result.M32 *= scale.Z;
            result.M33 *= scale.Z;

            // Добавляем перемещение
            result.M41 = translation.X;
            result.M42 = translation.Y;
            result.M43 = translation.Z;

            return result;
        }

        public static void Decompose(this Matrix4x4 m, out Vector3 translation, out Quaternion rotation, out Vector3 scale)
        {
            translation = m.GetTranslation();
            rotation = m.GetRotation();
            scale = m.GetScale();
        }
    }
}