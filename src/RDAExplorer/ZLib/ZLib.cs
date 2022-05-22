using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO.Compression;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib;

namespace RDAExplorer.ZLib
{
    public class ZLib
    {
        private static readonly int CompressionLevel = 2;

        public static byte[] Uncompress(byte[] input, int uncompressedSize)
        {
            return Uncompress(input, uncompressedSize, out int _);
        }

        public static byte[] Uncompress(byte[] input, int uncompressedSize, out int result)
        {
            try
            {
                Stream stream = new MemoryStream(input);
                stream.Position = 0;

                var decompressionStream = new InflaterInputStream(stream);
                var decompressedFileStream = new MemoryStream(uncompressedSize);

                decompressionStream.CopyTo(decompressedFileStream);

                byte[] bytes = decompressedFileStream.ToArray();

                result = (int)decompressedFileStream.Length;

                decompressedFileStream.Dispose();
                decompressionStream.Dispose();
                stream.Dispose();

                return bytes;
            }
            catch (SharpZipBaseException e)
            {
                result = 0;
                return new byte[0];
            }
        }

        public static byte[] Compress(byte[] input)
        {
            try
            {
                Stream stream = new MemoryStream(input);

                var memoryStream = new MemoryStream();
                var deflaterStream = new DeflaterOutputStream(memoryStream, new Deflater(CompressionLevel));

                //write input stream to the deflater stream 
                stream.Position = 0;
                stream.CopyTo(deflaterStream);
                deflaterStream.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Dispose();
                deflaterStream.Dispose();
                stream.Dispose();

                return bytes;
            }
            
            catch (SharpZipBaseException e)
            {
                return new byte[0];
            }
        }
    }
}
