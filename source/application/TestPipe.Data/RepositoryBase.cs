namespace TestPipe.DataAccess
{
	using System;

	public class RepositoryBase
	{
		public RepositoryBase()
		{
		}

		protected string ConnectionConfig { get; set; }

		protected NPoco.Database Db { get; set; }
	}
}