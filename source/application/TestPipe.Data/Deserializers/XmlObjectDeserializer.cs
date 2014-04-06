namespace TestPipe.Data.Deserializers
{
	using System;
	using System.IO;
	using System.Xml.Serialization;

	public class XmlObjectDeserializer
	{
		public T Deserialize<T>(string xml, T obj)
		{
			T deserialized = (T)obj;

			XmlSerializer serializer = new XmlSerializer(typeof(T));

			StringReader reader = new StringReader(xml);

			deserialized = (T)serializer.Deserialize(reader);

			return deserialized;
		}
	}
}