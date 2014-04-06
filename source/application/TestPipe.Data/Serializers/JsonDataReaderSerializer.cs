namespace TestPipe.Data.Serializers
{
	using System;
	using System.Data.Common;
	using System.Text;
	using System.Web;

	public class JsonDataReaderSerializer : BaseDataReaderSerializer, IDataSerializer
	{
		public string Serialize(object obj, string dataName)
		{
			DbDataReader reader = obj as DbDataReader;

			this.ValidateSerializeReader(reader, dataName);

			string data = string.Empty;

			if (reader.HasRows)
			{
				data = this.GetJsonObject(dataName, reader);
			}

			return data;
		}

		private string GetJsonKey(string key)
		{
			StringBuilder s = new StringBuilder();

			s.Append("\"");
			s.Append(key);
			s.Append("\"");
			s.Append(": ");

			return s.ToString();
		}

		private string GetJsonMember(DbDataReader reader, int i, int depth)
		{
			StringBuilder s = new StringBuilder();

			s.AppendLine(string.Empty);
			s.Append(this.GetTabDepth(depth));

			//column name
			s.Append(this.GetJsonKey(reader.GetName(i)));

			//column type
			Type type = reader.GetFieldType(i);

			//column value
			string columnValue = string.Empty;

			if (reader[i] != null & reader[i] != DBNull.Value)
			{
				if (type == typeof(string))
				{
					columnValue = HttpUtility.JavaScriptStringEncode(reader[i].ToString());
				}
				else
				{
					columnValue = reader[i].ToString();
				}
			}

			s.Append(this.GetJsonValue(columnValue, type));

			if (i + 1 == reader.FieldCount)
			{
				return s.ToString();
			}

			s.Append(",");

			return s.ToString();
		}

		private string GetJsonObject(string dataName, DbDataReader reader)
		{
			StringBuilder s = new StringBuilder();

			s.Append(this.GetJsonObjectHeader(dataName, 1));

			using (reader)
			{
				int rowCount = 0;
				while (reader.Read())
				{
					if (rowCount > 0)
					{
						s.Append(",");
					}

					s.Append(this.GetJsonObjectHeader(BaseDataReaderSerializer.RowName, 2));

					for (int i = 0; i < reader.FieldCount; i++)
					{
						s.Append(this.GetJsonMember(reader, i, 3));
					}

					s.Append(this.GetJsonObjectFooter(BaseDataReaderSerializer.RowName, 2));
					rowCount++;
				}
			}

			s.Append(this.GetJsonObjectFooter(dataName, 1));

			return s.ToString();
		}

		private string GetJsonObjectFooter(string dataName, int depth)
		{
			StringBuilder s = new StringBuilder();

			s.AppendLine(string.Empty);
			s.Append(this.GetTabDepth(depth));
			s.Append("}");

			return s.ToString();
		}

		private string GetJsonObjectHeader(string dataName, int depth)
		{
			StringBuilder s = new StringBuilder();

			s.AppendLine(string.Empty);
			s.Append(this.GetTabDepth(depth));
			s.Append(this.GetJsonKey(dataName));
			s.Append(" {");

			return s.ToString();
		}

		private string GetJsonValue(string value, Type type)
		{
			StringBuilder s = new StringBuilder();

			string quotes = string.Empty;

			if (type == typeof(string) || type == typeof(DateTime) || type == typeof(bool))
			{
				quotes = "\"";
			}

			if (type == typeof(bool))
			{
				value = value.ToLower();
			}

			s.Append(quotes);
			s.Append(value);
			s.Append(quotes);

			if (s.Length < 1)
			{
				s.Append("\"\"");
			}

			return s.ToString();
		}

		private string GetTabDepth(int depth)
		{
			StringBuilder s = new StringBuilder();

			for (int i = 0; i < depth; i++)
			{
				s.Append("\t");
			}

			return s.ToString();
		}
	}
}