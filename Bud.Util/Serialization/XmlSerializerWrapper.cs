namespace Bud.Util.Serialization
{
    using System.IO;
    using System.Xml.Serialization;

    public class XmlSerializerWrapper<T> : IXmlSerializerWrapper<T>
    {
        private readonly XmlSerializer _serializer;

        public XmlSerializerWrapper()
        {
            _serializer = new XmlSerializer(typeof(T));
        }

        public T Deserialize(Stream stream)
        {
            return (T)_serializer.Deserialize(stream);
        }
    }
}