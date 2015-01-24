using System;

namespace ShinyBackupTool
{
    [Serializable]
    class FileRecord
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public DateTime LastWrite { get; set; }
        public string Sha1Checksum { get; set; }

        public FileSample[] Samples { get; set; }

        public FileSample[] CalculateNewSamples(int howMany)
        {
            var samples = new FileSample[howMany];
            var length = Math.Min(Size / howMany, 4 * 1024); // maximum sample size = 4 kB
            var offsetMultiplier = (Size - length) / howMany;
            for (int i = 0; i < howMany; i++)
            {
                var sample = new FileSample();
                sample.Offset = i * offsetMultiplier;
                sample.Length = length;
                samples[i] = sample;
            }
            return samples;
        }
    }
}