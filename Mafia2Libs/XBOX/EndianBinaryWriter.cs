using System;
using System.IO;


namespace XBOX
{
    public class EndianBinaryWriter : BinaryWriter
    {
        private readonly bool _isBigEndian;
        public bool IsBigEndian => _isBigEndian;

        public EndianBinaryWriter(Stream output, bool isBigEndian) : base(output)
        {
            _isBigEndian = isBigEndian;
        }

        private void WriteReversed(byte[] bytes)
        {
            if (_isBigEndian) Array.Reverse(bytes);
            base.Write(bytes);
        }

        public override void Write(short value) => WriteReversed(BitConverter.GetBytes(value));
        public override void Write(ushort value) => WriteReversed(BitConverter.GetBytes(value));
        public override void Write(int value) => WriteReversed(BitConverter.GetBytes(value));
        public override void Write(uint value) => WriteReversed(BitConverter.GetBytes(value));
        public override void Write(long value) => WriteReversed(BitConverter.GetBytes(value));
        public override void Write(ulong value) => WriteReversed(BitConverter.GetBytes(value));
        public override void Write(float value) => WriteReversed(BitConverter.GetBytes(value));
        public override void Write(double value) => WriteReversed(BitConverter.GetBytes(value));
    }
}
