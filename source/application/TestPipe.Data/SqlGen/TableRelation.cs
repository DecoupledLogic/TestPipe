namespace TestPipe.Data.SqlGen
{
	using System;

	public class TableRelation
	{
		public string Join { get; set; }

		public string RelatedOn { get; set; }

		public Table RelatedTable { get; set; }

		public Table Table { get; set; }
	}
}