using System;
using System.IO;


namespace XBOX
{
    public class EndianBinaryReader : BinaryReader
    {
        private readonly bool _isBigEndian;

        public EndianBinaryReader(Stream input, bool isBigEndian) : base(input)
        {
            _isBigEndian = isBigEndian;
        }

        private byte[] ReadBytesAndReverse(int count)
        {
            var bytes = base.ReadBytes(count);
            if (bytes.Length != count)
                throw new EndOfStreamException($"Expected {count} bytes, got {bytes.Length}. Stream position: {BaseStream.Position}");
            if (_isBigEndian) Array.Reverse(bytes);
            return bytes;
        }

        public override short ReadInt16() => BitConverter.ToInt16(ReadBytesAndReverse(2), 0);
        public override ushort ReadUInt16() => BitConverter.ToUInt16(ReadBytesAndReverse(2), 0);
        public override int ReadInt32() => BitConverter.ToInt32(ReadBytesAndReverse(4), 0);
        public override uint ReadUInt32() => BitConverter.ToUInt32(ReadBytesAndReverse(4), 0);
        public override long ReadInt64() => BitConverter.ToInt64(ReadBytesAndReverse(8), 0);
        public override ulong ReadUInt64() => BitConverter.ToUInt64(ReadBytesAndReverse(8), 0);
        public override float ReadSingle() => BitConverter.ToSingle(ReadBytesAndReverse(4), 0);
        public override double ReadDouble() => BitConverter.ToDouble(ReadBytesAndReverse(8), 0);
    }
}