using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AimlWPF.Models
{
    class XmlWorker
    {
        public static void serialize(object item, string path)
        {
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            StreamWriter writer = new StreamWriter(path);
            serializer.Serialize(writer.BaseStream, item);
            writer.Close();
        }

        public static T deserialize<T>(string path)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            StreamReader reader = new StreamReader(path);
            try
            {
                T deserialize = (T)xml.Deserialize(reader.BaseStream);
                reader.Close();
                return deserialize;
            }
            catch (Exception e)
            {
                reader.Close();
            }
            reader.Close();
            return default(T);
        }
    }
}
