namespace TestPipe.Data.SqlGen
{
	using System;
	using System.Collections.Generic;

	public class DataQuery
	{
		public DataQuery()
		{
			this.Parameters = new List<Parameter>();
			this.Tables = new List<Table>();
			this.Relations = new List<TableRelation>();
			this.RelatedSql = new Dictionary<string, string>();
		}

		public string FromSql { get; set; }

		public Table FromTable { get; set; }

		public string Order { get; set; }

		public ICollection<Parameter> Parameters { get; set; }

		public string Query { get; set; }

		public Dictionary<string, string> RelatedSql { get; set; }

		public ICollection<TableRelation> Relations { get; set; }

		public ICollection<Table> Tables { get; set; }

		public string Where { get; set; }
	}
}