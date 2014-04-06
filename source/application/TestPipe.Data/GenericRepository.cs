namespace TestPipe.Data
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using TestPipe.DataAccess;

	public class GenericRepository<T> : RepositoryBase where T : class
	{
		public GenericRepository(string connectionConfig)
			: base()
		{
			if (string.IsNullOrWhiteSpace(connectionConfig))
			{
				this.ConnectionConfig = "TestPipe";
			}

			this.ConnectionConfig = connectionConfig;

			ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[this.ConnectionConfig];

			this.ConnectionString = settings.ConnectionString;
			this.ProviderName = settings.ProviderName;

			this.Db = new NPoco.Database(this.ConnectionString, this.ProviderName);
		}

		public GenericRepository(string connectionString, string providerName)
		{
			this.ProviderName = providerName;
			this.ConnectionString = connectionString;
			this.Db = new NPoco.Database(this.ConnectionString, this.ProviderName);
		}

		public string ConnectionString { get; set; }

		public string ProviderName { get; set; }

		public void Delete(int id)
		{
			try
			{
				using (this.Db)
				{
					this.Db.Delete<T>(id);
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException(System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
			}
		}

		public T Insert(T dto)
		{
			if (dto == null)
			{
				throw new ArgumentNullException("dto");
			}

			try
			{
				using (this.Db)
				{
					this.Db.Insert<T>(dto);
					return dto;
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException(System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
			}
		}

		public List<T> Select(System.Linq.Expressions.Expression<Func<T, bool>> expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression can not be a null value.");
			}

			try
			{
				using (this.Db)
				{
					return this.Db.FetchWhere<T>(expression);
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException(System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
			}
		}

		public T SelectSingle(int id)
		{
			if (id < 1)
			{
				throw new ArgumentException("id < 1");
			}

			try
			{
				using (this.Db)
				{
					return this.Db.SingleById<T>(id);
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException(System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
			}
		}

		public void Save(T dto)
		{
			if (dto == null)
			{
				throw new ArgumentNullException("dto");
			}

			try
			{
				using (this.Db = new NPoco.Database(this.ConnectionString, this.ProviderName))
				{
					this.Db.Save<T>(dto);
				}
			}
			catch (Exception ex)
			{
				throw new ApplicationException(System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
			}
		}
	}
}