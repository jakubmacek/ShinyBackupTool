using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ShinyBackupTool
{
    class Database
    {
        [Serializable]
        private class DataStructure
        {
            public List<FileRecord> Files { get; set; }

            public DataStructure()
            {
                Files = new List<FileRecord>();
            }
        }

        private DataStructure Data { get; set; }

        public Database()
        {
            Data = new DataStructure();
        }

        public void Load(string filePath)
        {
            Load(new FileStream(filePath, FileMode.Open));
        }

        public void Load(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(DataStructure));
            Data = (DataStructure) serializer.Deserialize(stream);
        }

        public void Save(string filePath)
        {
            Save(new FileStream(filePath, FileMode.Create));
        }

        public void Save(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(DataStructure));
            serializer.Serialize(stream, Data);
        }
    }
}
