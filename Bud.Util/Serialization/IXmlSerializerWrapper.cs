namespace Bud.Util.Serialization
{
    using System.IO;

    public interface IXmlSerializerWrapper<T>
    {
        T Deserialize(Stream stream);
    }
}