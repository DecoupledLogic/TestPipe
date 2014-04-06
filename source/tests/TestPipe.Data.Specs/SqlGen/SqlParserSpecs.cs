namespace TestPipe.Data.Specs.SqlGen
{
	using System;
	using System.Linq;
	using System.Text;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Data.SqlGen;

	[TestClass]
	public class SqlParserSpecs
	{
		private static readonly string From;

		private static readonly string FromTable;

		private static readonly string FromTableAlias;

		private static readonly string Join1;

		private static readonly string JoinColumn;

		private static readonly string RelatedOn;

		private static readonly string RelatedTable;

		private static readonly string RelatedTableAlias;

		private static readonly string WhereClause;

		private static readonly string WhereStatement;

		private DataQuery query;
		private SqlParser sut;

		static SqlParserSpecs()
		{
			FromTable = "Customer";
			FromTableAlias = "c";
			From = "from " + FromTable + " " + FromTableAlias;
			RelatedTable = "User";
			RelatedTableAlias = "u1";
			RelatedOn = FromTableAlias + JoinColumn + " = " + RelatedTableAlias + JoinColumn;
			Join1 = "inner join " + RelatedTable + " " + RelatedTableAlias + " on " + RelatedOn;
			JoinColumn = ".UserId";
			WhereClause = " " + FromTableAlias + JoinColumn + " = '41367f65'";
			WhereStatement = "where" + WhereClause;
		}

		[TestMethod]
		public void GetFromSqlReturnsRelatedSql()
		{
			string where = this.GetWhereString();
			this.query = this.sut.ProcessQuery(where);
			string expected = "select u1.* from Customer c\r\ninner join User u1 on c.UserId = u1.UserId\r\ninner join User u2 on u1.UserId = u2.UserId\r\ninner join Customer c on u2.UserId = c.UserId\r\ninner join TaskRecord tr on u2.UserId = tr.UserId\r\ninner join TestPipe..BillingSummary summary on c.CustomerId = summary.CustomerId and summary.Status not in ('fully_settled')\r\ninner join TestPipe..BillingStatus stat on summary.[Status] = stat.BillingStatus\r\ninner join Invoice i on c.CustomerId = i.CustomerId\r\ninner join Task t on i.InvoiceId = t.InvoiceId\r\nwhere c.CustomerId = '41367f65'\r\n";

			string actual = this.sut.GetRelatedSql(this.query, "u1").RelatedSql.Where(x => x.Key == "u1").FirstOrDefault().Value;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetFromSqlReturnsSql()
		{
			string where = this.GetWhereString();
			this.query = this.sut.ProcessQuery(where);
			string expected = "select u1.* from Customer c\r\ninner join User u1 on c.UserId = u1.UserId\r\ninner join User u2 on u1.UserId = u2.UserId\r\ninner join Customer c on u2.UserId = c.UserId\r\ninner join TaskRecord tr on u2.UserId = tr.UserId\r\ninner join TestPipe..BillingSummary summary on c.CustomerId = summary.CustomerId and summary.Status not in ('fully_settled')\r\ninner join TestPipe..BillingStatus stat on summary.[Status] = stat.BillingStatus\r\ninner join Invoice i on c.CustomerId = i.CustomerId\r\ninner join Task t on i.InvoiceId = t.InvoiceId\r\nwhere c.CustomerId = '41367f65'\r\n";

			string actual = this.sut.GetFromSql(this.query).FromSql;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetTableReturnsDatabaseName()
		{
			string expected = "TestPipe";
			Table table = new Table();

			Table actual = this.sut.GetTable(table, "[TestPipe].[dbo].[Customer]");

			Assert.AreEqual(expected, actual.DatabaseName);
		}

		[TestMethod]
		public void GetTableReturnsSchemaName()
		{
			string expected = "dbo";
			Table table = new Table();

			Table actual = this.sut.GetTable(table, "[TestPipe].[dbo].[Customer]");

			Assert.AreEqual(expected, actual.SchemaName);
		}

		[TestMethod]
		public void GetTableReturnsTableName()
		{
			string expected = "Customer";
			Table table = new Table();

			Table actual = this.sut.GetTable(table, "[TestPipe].[dbo].[Customer]");

			Assert.AreEqual(expected, actual.Name);
		}

		[TestMethod]
		public void GetTableWithEmptySchemaNameReturnsDatabaseName()
		{
			string expected = "TestPipe";
			Table table = new Table();

			Table actual = this.sut.GetTable(table, "[TestPipe]..[Customer]");

			Assert.AreEqual(expected, actual.DatabaseName);
		}

		[TestMethod]
		public void GetTableWithEmptySchemaNameReturnsEmptySchemaName()
		{
			string expected = string.Empty;
			Table table = new Table();

			Table actual = this.sut.GetTable(table, "[TestPipe]..[Customer]");

			Assert.AreEqual(expected, actual.SchemaName);
		}

		[TestMethod]
		public void ProcessLineWithFromAndJoinLineReturnsDataQueryWithFromTableAlias()
		{
			this.query = this.sut.ProcessLine(From, this.query);
			this.query = this.sut.ProcessLine(Join1, this.query);

			Table actual = this.query.FromTable;

			Assert.AreEqual(FromTableAlias, actual.Alias);
		}

		[TestMethod]
		public void ProcessLineWithFromAndJoinLineReturnsDataQueryWithFromTableName()
		{
			this.query = this.sut.ProcessLine(From, this.query);
			this.query = this.sut.ProcessLine(Join1, this.query);

			Table actual = this.query.FromTable;

			Assert.AreEqual(FromTable, actual.Name);
		}

		[TestMethod]
		public void ProcessLineWithFromAndJoinLineReturnsDataQueryWithRelatedTableAlias()
		{
			this.query = this.sut.ProcessLine(From, this.query);
			this.query = this.sut.ProcessLine(Join1, this.query);

			Table actual = this.query.Tables.Where(x => x.Alias == RelatedTableAlias).FirstOrDefault();

			Assert.AreEqual(RelatedTableAlias, actual.Alias);
		}

		[TestMethod]
		public void ProcessLineWithFromAndJoinLineReturnsDataQueryWithRelatedTableName()
		{
			this.query = this.sut.ProcessLine(From, this.query);
			this.query = this.sut.ProcessLine(Join1, this.query);

			Table actual = this.query.Tables.Where(x => x.Name == RelatedTable).FirstOrDefault();

			Assert.AreEqual(RelatedTable, actual.Name);
		}

		[TestMethod]
		public void ProcessLineWithFromAndJoinLineReturnsDataQueryWithRelatedTablesRelatedOn()
		{
			this.query = this.sut.ProcessLine(From, this.query);
			this.query = this.sut.ProcessLine(Join1, this.query);

			TableRelation actual = this.query.Relations.FirstOrDefault();

			Assert.AreEqual(RelatedOn, actual.RelatedOn);
		}

		[TestMethod]
		public void ProcessLineWithFromAndJoinLineReturnsDataQueryWithRelatedTablesRelatedTableName()
		{
			this.query = this.sut.ProcessLine(From, this.query);
			this.query = this.sut.ProcessLine(Join1, this.query);

			TableRelation actual = this.query.Relations.FirstOrDefault();

			Assert.AreEqual(RelatedTable, actual.RelatedTable.Name);
		}

		[TestMethod]
		public void ProcessLineWithFromAndJoinLineReturnsDataQueryWithRelatedTablesTableName()
		{
			this.query = this.sut.ProcessLine(From, this.query);
			this.query = this.sut.ProcessLine(Join1, this.query);

			TableRelation actual = this.query.Relations.FirstOrDefault();

			Assert.AreEqual(FromTable, actual.Table.Name);
		}

		[TestMethod]
		public void ProcessLineWithFromJoinAndWhereLineReturnsDataQueryWithWhere()
		{
			this.query = this.sut.ProcessLine(From, this.query);
			this.query = this.sut.ProcessLine(Join1, this.query);
			this.query = this.sut.ProcessLine(WhereStatement, this.query);

			string actual = this.query.Where;

			Assert.AreEqual(WhereClause, actual);
		}

		[TestMethod]
		public void ProcessLineWithFromLineReturnsDataQueryTableAlias()
		{
			this.query = this.sut.ProcessLine(From, this.query);

			Table actual = this.query.FromTable;

			Assert.AreEqual(FromTableAlias, actual.Alias);
		}

		[TestMethod]
		public void ProcessLineWithFromLineReturnsDataQueryTableName()
		{
			this.query = this.sut.ProcessLine(From, this.query);

			Table actual = this.query.FromTable;

			Assert.AreEqual(FromTable, actual.Name);
		}

		[TestMethod]
		public void ProcessLineWithFromLineWithBracketsAndPrefixesReturnsDataQueryTableName()
		{
			this.query = this.sut.ProcessLine("FROM [TestPipe].[dbo].[Customer] c", this.query);
			string expected = "TaskRecord";

			Table actual = this.query.FromTable;

			Assert.AreEqual(expected, actual.Name);
		}

		[TestMethod]
		public void ProcessLineWithJoinLineReturnsDataQueryTableAlias()
		{
			this.query = this.sut.ProcessLine(From, this.query);
			this.query = this.sut.ProcessLine(Join1, this.query);

			Table actual = this.query.Tables.Where(x => x.Alias == RelatedTableAlias).FirstOrDefault();

			Assert.AreEqual(RelatedTableAlias, actual.Alias);
		}

		[TestMethod]
		public void ProcessLineWithJoinLineReturnsDataQueryTableName()
		{
			this.query = this.sut.ProcessLine(From, this.query);
			this.query = this.sut.ProcessLine(Join1, this.query);

			Table actual = this.query.Tables.Where(x => x.Name == RelatedTable).FirstOrDefault();

			Assert.AreEqual(RelatedTable, actual.Name);
		}

		[TestMethod]
		public void ProcessLineWithJoinLineWithBracketsAndPrefixesReturnsDataQueryTableName()
		{
			this.query = this.sut.ProcessLine("FROM [TestPipe].[dbo].[Customer] c", this.query);
			this.query = this.sut.ProcessLine("INNER JOIN [TestPipe].[dbo].[User] u on c.[UserId] = u.[UserId]", this.query);
			string expected = "User";

			Table actual = this.query.Tables.Where(x => x.Alias == "u").FirstOrDefault();

			Assert.AreEqual(expected, actual.Name);
		}

		[TestMethod]
		public void ProcessQueryReturnsDataQuery()
		{
			string where = this.GetWhereString();

			this.query = this.sut.ProcessQuery(where);

			Assert.AreEqual(WhereClause, this.query.Where);
		}

		[TestInitialize]
		public void SetUp()
		{
			this.sut = new SqlParser();
			this.query = new DataQuery();
		}

		private string GetWhereString()
		{
			StringBuilder where = new StringBuilder();

			where.AppendLine("from Customer c");
			where.AppendLine("inner join User u1 on c.CustomerId = u1.UserId");
			where.AppendLine("inner join User u2 on u1.UserId = u2.UserId");
			where.AppendLine("inner join Customer c on u2.UserId = c.UserId");
			where.AppendLine("inner join TaskRecord tr on u2.UserId = tr.UserId");
			where.AppendLine("inner join TestPipe..BillingSummary summary on c.CustomerId = summary.CustomerId and summary.Status not in ('fully_settled')");
			where.AppendLine("inner join TestPipe..BillingStatus stat on summary.[Status] = stat.BillingStatus");
			where.AppendLine("inner join Invoice i on c.CustomerId = i.CustomerId");
			where.AppendLine("inner join Task t on i.InvoiceId = t.InvoiceId");
			where.AppendLine("where c.CustomerId = '41367f65'");

			return where.ToString();
		}
	}
}