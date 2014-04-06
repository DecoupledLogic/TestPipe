namespace TestPipe.Data.Serializers
{
	using System;
	using System.IO;
	using System.Xml.Serialization;

	public class XmlObjectSerializer : BaseDataObjectSerializer, IDataSerializer
	{
		public string Serialize(object obj, string dataName)
		{
			this.ValidateSerializeObject(obj, dataName);

			string data = this.GetXmlObject(dataName, obj);

			return data;
		}

		public string GetXmlObject(string dataName, object obj)
		{
			Type type = Type.GetType("TestPipe.Data.Transfer.Dto." + dataName);

			XmlSerializer serializer = new XmlSerializer(type);

			using (StringWriter writer = new StringWriter())
			{
				serializer.Serialize(writer, obj);

				return writer.ToString();
			}
		}
	}
}