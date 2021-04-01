using System.IO;
using System.Text;

namespace Infrastructure.Transformation
{
    public interface IXmlSerializer
    {
        string ToXml(params object[] value);
    }

    public class XmlSerializer : IXmlSerializer
    {
        private const string ObjectIsNull = "Object is NULL";

        public T FromXml<T>(string xml)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(memoryStream);
            }
        }

        public byte[] ToXmlAsByteArray(object source)
        {
            return Encoding.UTF8.GetBytes(ToXml(source));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public string ToXml(params object[] value)
        {
            if (value == null) return ObjectIsNull;

            var ms = new MemoryStream();

            using (var sw = new StreamWriter(ms))
            {
                foreach (object serializableObject in value)
                {
                    if (serializableObject == null)
                    {
                        AddNullToOutput(sw);
                    }
                    else
                    {
                        AddSerializedObjectToOutput(serializableObject, sw);
                    }
                }

                sw.Flush();
                ms.Position = 0;
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private static void AddSerializedObjectToOutput(object serializableObject, StreamWriter sw)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(serializableObject.GetType());
            {
                serializer.Serialize(sw, serializableObject);
            }
        }

        private static void AddNullToOutput(StreamWriter sw)
        {
            sw.WriteLine(ObjectIsNull);
        }
    }
}
