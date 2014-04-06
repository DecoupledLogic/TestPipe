namespace TestPipe.Data.SqlGen
{
	using System;
	using System.Linq;
	using System.Text;

	public class SqlParser
	{
		/*
		 * This class was created to parse the SQL statements below. It really should use RegEx so if you want to refactor have at it, just make sure the specs pass while you do it.
			 * From
			 * Join
			 * Where
		   * Order - this is include in the code, but we don't have a use case for it yet.
					from Customer c
					inner join User u1 on c.CustomerId = u1.UserId
					inner join User u2 on u1.UserId = u2.UserId
					inner join Customer c on u2.UserId = c.UserId
					inner join TaskRecord tr on u2.UserId = tr.UserId
					inner join TestPipe..BillingSummary summary on c.CustomerId = summary.CustomerId and summary.Status not in ('fully_settled')
					inner join TestPipe..BillingStatus stat on summary.[Status] = stat.BillingStatus
					inner join Invoice i on c.CustomerId = i.CustomerId
					inner join Task t on i.InvoiceId = t.InvoiceId
					where c.CustomerId = '41367f65'

		*/

		private const string OrderClause = "orderclause";
		private const string WhereClause = "whereclause";

		public SqlParser()
		{
		}

		public DataQuery GetFromSql(DataQuery query)
		{
			if (string.IsNullOrWhiteSpace(query.FromTable.Alias))
			{
				throw new ArgumentException("DataQuery.FromTable.Alias is null or white space.");
			}

			string sql = this.GetSelectSql(query, query.FromTable.Alias);

			query.FromSql = sql;
			return query;
		}

		public DataQuery GetRelatedSql(DataQuery query, string relatedAlias)
		{
			if (string.IsNullOrWhiteSpace(relatedAlias))
			{
				throw new ArgumentException("Parameter relatedAlias is null or white space.");
			}

			string sql = this.GetSelectSql(query, relatedAlias);

			query.RelatedSql.Add(relatedAlias, sql);
			return query;
		}

		public DataQuery GetSql(DataQuery query)
		{
			query = this.GetFromSql(query);

			foreach (TableRelation relation in query.Relations)
			{
				query = this.GetRelatedSql(query, relation.RelatedTable.Alias);
			}

			return query;
		}

		public DataQuery ProcessLine(string line, DataQuery query)
		{
			string[] sections = line.ToLower().Split(' ');
			string expect = string.Empty;
			string entity = string.Empty;
			string leftTable = string.Empty;
			string leftAlias = string.Empty;
			string rightTable = string.Empty;
			StringBuilder where = new StringBuilder();
			StringBuilder order = new StringBuilder();

			foreach (var section in sections)
			{
				if (expect == "end")
				{
					break;
				}
				
				string term = section.Trim().Replace("\t", string.Empty);
				if (string.IsNullOrWhiteSpace(term))
				{
					continue;
				}

				if (string.IsNullOrWhiteSpace(expect))
				{
					expect = this.GetExpectFromLine(term);

					if (!string.IsNullOrWhiteSpace(expect))
					{
						continue;
					}
				}

				Table table = new Table();
				TableRelation relation = new TableRelation();
				string[] tableColumn;

				switch (expect.ToLower())
				{
					case "fromtablesource":
						table = this.GetTable(table, term);
						entity = table.Name;
						query.FromTable = table;
						query.Tables.Add(table);
						expect = "fromtablealias";
						break;

					case "table":
						table = this.GetTable(table, term);
						entity = table.Name;
						query.Tables.Add(table);
						expect = "alias";
						break;

					case "fromtablealias":
						table = query.FromTable;
						if (table == null)
						{
							break;
						}
						table.Alias = term;
						expect = string.Empty;
						entity = string.Empty;
						break;

					case "alias":
						table = query.Tables.Where(x => x.Name == entity).LastOrDefault();
						if (table == null)
						{
							break;
						}
						table.Alias = term;
						expect = string.Empty;
						entity = string.Empty;
						break;

					case "lefttablesource":
						leftTable = term;
						tableColumn = term.Split('.');
						leftAlias = tableColumn[0];
						table = query.Tables.Where(x => x.Alias == leftAlias).FirstOrDefault();
						if (table == null)
						{
							throw new ApplicationException("The table for the left table relation is not defined.");
						}
						relation.Table = table;
						relation.Join = line;
						query.Relations.Add(relation);
						expect = string.Empty;
						entity = string.Empty;
						break;

					case "righttablesource":
						rightTable = term;
						tableColumn = term.Split('.');
						table = query.Tables.Where(x => x.Alias == tableColumn[0]).FirstOrDefault();
						if (table == null)
						{
							throw new ApplicationException("The table for the right table relation is not defined.");
						}
						relation = query.Relations.Where(x => x.Table.Alias == leftAlias).FirstOrDefault();
						relation.RelatedTable = table;
						relation.RelatedOn = string.Format("{0} = {1}", leftTable, rightTable);
						expect = "end";
						entity = string.Empty;
						break;

					case WhereClause:
						where.Append(" ");
						where.Append(term);
						expect = WhereClause;
						entity = string.Empty;
						break;

					case OrderClause:
						if (term == "by")
						{
							break;
						}
						order.Append(" ");
						order.Append(term);
						expect = OrderClause;
						entity = string.Empty;
						break;
				}
			}

			if (expect == WhereClause)
			{
				query.Where = where.ToString();
			}

			if (expect == OrderClause)
			{
				query.Order = order.ToString();
			}

			return query;
		}

		public DataQuery ProcessQuery(string where)
		{
			DataQuery query = new DataQuery();

			query.Query = where;

			string[] lines = where.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

			foreach (var line in lines)
			{
				query = this.ProcessLine(line, query);
			}

			return query;
		}

		public Table GetTable(Table table, string term)
		{
			table.Name = this.GetTableName(term);
			table.DatabaseName = this.GetDatabaseName(term);
			table.SchemaName = this.GetSchemaName(term);
			return table;
		}

		private string GetTableName(string name)
		{
			string[] names = this.GetTableNameParts(name);
			name = names.Last();
			return name;
		}

		private string GetSchemaName(string tableName)
		{
			string[] names = this.GetTableNameParts(tableName);
			if (names.Length == 3 && !string.IsNullOrWhiteSpace(names[1]))
			{
				return names[1];
			}
			return string.Empty;
		}

		private string GetDatabaseName(string tableName)
		{
			string[] names = this.GetTableNameParts(tableName);
			if (names.Length == 3 && !string.IsNullOrWhiteSpace(names[0]))
			{
				return names[0];
			}
			return string.Empty;
		}

		private string[] GetTableNameParts(string name)
		{
			name = name.Trim().Replace("[", string.Empty).Replace("]", string.Empty).ToLower();
			string[] names = name.Split('.');
			return names;
		}

		private string GetExpectFromLine(string line)
		{
			switch (line.ToLower())
			{
				case "from":
					return "fromtablesource";

				//case "inner":
				//case "left":
				//case "right":
				//case "outer":
				case "join":
					return "table";

				case "on":
					return "lefttablesource";

				case "=":
					return "righttablesource";

				case "where":
					return WhereClause;

				case "order":
				case "by":
					return OrderClause;
			}

			return string.Empty;
		}

		private string GetSelectSql(DataQuery query, string alias)
		{
			StringBuilder sql = new StringBuilder();

			sql.Append("select ");
			sql.Append(alias);
			sql.Append(".* ");
			sql.Append(query.Query);

			return sql.ToString();
		}
	}
}