using System;
using System.IO;
using System.Security.Cryptography;

namespace ShinyBackupTool
{
    [Serializable]
    class FileSample
    {
        public long Offset { get; set; }
        public long Length { get; set; }
        public string Sha1Checksum { get; set; }

        public byte[] ExtractBytes(Stream stream)
        {
            var buffer = new byte[Length];
            stream.Seek(Offset, SeekOrigin.Begin);
            stream.Read(buffer, 0, (int)Length);
            return buffer;
        }

        public void ComputeChecksum(Stream stream)
        {
            using (var cryptoProvider = new SHA1CryptoServiceProvider())
            {
                Sha1Checksum = BitConverter.ToString(cryptoProvider.ComputeHash(ExtractBytes(stream)));
            }
        }
    }
}