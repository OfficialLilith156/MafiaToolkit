using System;
using System.IO;

public static class XboxDdsToPc
{
    public static void Convert(string inputPath, string outputPath)
    {
        byte[] data = File.ReadAllBytes(inputPath);
        if (data.Length < 128)
            throw new Exception("File is too small for a DDS header.");

        if (data[0] != 0x44 || data[1] != 0x44 || data[2] != 0x53 || data[3] != 0x20)
            throw new Exception("Not a DDS file.");

        uint dwSize = BitConverter.ToUInt32(data, 4);
        bool isXbox = (dwSize == 0x7C000000);

        if (!isXbox)
        {
            File.Copy(inputPath, outputPath, true);
            return;
        }

        byte[] header = new byte[128];
        Array.Copy(data, header, 128);
        for (int offset = 4; offset < 128; offset += 4)
            Array.Reverse(header, offset, 4);

        int dataOffset = 128;
        int dataLength = data.Length - dataOffset;
        byte[] pixelData = new byte[dataLength];
        Array.Copy(data, dataOffset, pixelData, 0, dataLength);

        string fourCC = System.Text.Encoding.ASCII.GetString(header, 84, 4);
        bool isCompressed = fourCC.StartsWith("DXT");
        if (isCompressed)
        {
            int blockSize = (fourCC == "DXT1") ? 8 : 16;
            for (int i = 0; i < dataLength; i += 2)
            {
                byte temp = pixelData[i];
                pixelData[i] = pixelData[i + 1];
                pixelData[i + 1] = temp;
            }
        }

        byte[] output = new byte[128 + dataLength];
        Array.Copy(header, 0, output, 0, 128);
        Array.Copy(pixelData, 0, output, 128, dataLength);
        File.WriteAllBytes(outputPath, output);
    }
}