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
    }
}