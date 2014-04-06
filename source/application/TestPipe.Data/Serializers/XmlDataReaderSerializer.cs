namespace TestPipe.Data.Serializers
{
	using System;
	using System.Data.Common;
	using System.Xml.Linq;

	public class XmlDataReaderSerializer : BaseDataReaderSerializer, IDataSerializer
	{
		public string Serialize(object obj, string dataName)
		{
			DbDataReader reader = obj as DbDataReader;

			this.ValidateSerializeReader(reader, dataName);

			string dump = string.Empty;

			if (reader.HasRows)
			{
				dump = this.GetXmlObject(dataName, reader);
			}

			return dump;
		}

		private string GetXmlObject(string dataName, DbDataReader reader)
		{
			XElement results = new XElement(dataName);
			XElement result = new XElement(RowName);

			using (reader)
			{
				while (reader.Read())
				{
					for (int i = 0; i < reader.FieldCount; i++)
					{
						if (reader.GetName(i) != string.Empty)
						{
							if (reader[i] == null)
							{
								result.Add(new XElement(reader.GetName(i), "NULL"));
							}
							else
							{
								result.Add(new XElement(reader.GetName(i), reader[i].ToString()));
							}
						}
					}
				}
				results.Add(new XElement(result));
			}

			return results.ToString();
		}
	}
}