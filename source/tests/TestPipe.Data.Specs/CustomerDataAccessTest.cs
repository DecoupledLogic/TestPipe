namespace TestPipe.Data.Specs
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Data.JobSystem;
	using TestPipe.Data.Transfer.Dto.JobSystem;

	[TestClass]
	public class CustomerRepositoryTest
	{
		private string address;
		private string city;
		private string companyName;
		private string country;
		private string fileTypeDescription;
		private int resellerId;
		private string state;
		private CustomerRepository sut;
		private TestContext testContextInstance;
		private string zip;

		public CustomerRepositoryTest()
		{
		}

		public TestContext TestContext
		{
			get
			{
				return this.testContextInstance;
			}
			set
			{
				this.testContextInstance = value;
			}
		}

		public int AddCustomer(int resellerId, string companyName, string address, string city, string state, string zip, string country)
		{
			Customer dto = this.GetCustomer(resellerId, companyName, address, city, state, zip, country);
			Customer result = this.sut.AddCustomer(dto);
			return result.Customer_ID;
		}

		[TestMethod]
		public void AddCustomer_Should_Add_Customer()
		{
			Customer expected = this.GetCustomer(this.resellerId, this.companyName, this.address, this.city, this.state, this.zip, this.country);

			Customer actual = this.sut.AddCustomer(expected);

			Assert.AreEqual(expected.Row_Created_Date, actual.Row_Created_Date);
			Assert.IsTrue(actual.Customer_ID > 0);
		}

		public int AddCustomerFileType(string fileTypeDescription, int? customerId = null)
		{
			Customer_FileType dto = this.GetCustomer_FileType(fileTypeDescription, customerId);
			Customer_FileType result = this.sut.AddCustomerFileType(dto);
			return result.FileType_ID;
		}

		[TestMethod]
		public void AddCustomerFileType_Should_Add_CustomerFileType()
		{
			Customer_FileType expected = this.GetCustomer_FileType(this.fileTypeDescription);

			Customer_FileType actual = this.sut.AddCustomerFileType(expected);

			Assert.AreEqual(expected.Row_Created_Date, actual.Row_Created_Date);
			Assert.IsTrue(actual.Customer_ID > 0);
		}

		public Customer GetCustomer(int resellerId, string companyName, string address, string city, string state, string zip, string country)
		{
			Customer dto = new Customer();
			dto.Company_Name = companyName;
			dto.Reseller_ID = resellerId;
			dto.Address_1 = address;
			dto.City = city;
			dto.Zip = zip;
			dto.Country = country;
			dto.State = state;
			DateTime date = DateTime.Now;
			dto.Row_Created_Date = date;

			return dto;
		}

		public Customer_FileType GetCustomer_FileType(string fileTypeDescription, int? customerId = null)
		{
			if (!customerId.HasValue)
			{
				customerId = this.AddCustomer(this.resellerId, this.companyName, this.address, this.city, this.state, this.zip, this.country);
			}

			Customer_FileType dto = new Customer_FileType();
			dto.Customer_ID = customerId.Value;
			dto.FileType_Description = fileTypeDescription;
			DateTime date = DateTime.Now;
			dto.Row_Created_Date = date;

			return dto;
		}

		[TestMethod]
		public void GetCustomer_Should_Return_Customer()
		{
			int customerId = this.AddCustomer(this.resellerId, this.companyName, this.address, this.city, this.state, this.zip, this.country);

			Customer actual = this.sut.GetCustomer(customerId);

			Assert.AreEqual(customerId, actual.Customer_ID);
		}

		[TestMethod]
		public void GetCustomerFileType_Should_Return_CustomerFileType()
		{
			int customerFileTypeId = this.AddCustomerFileType(this.fileTypeDescription);

			Customer_FileType actual = this.sut.GetCustomerFileType(customerFileTypeId);

			Assert.AreEqual(customerFileTypeId, actual.FileType_ID);
		}

		[TestInitialize]
		public void MyTestInitialize()
		{
			this.sut = new CustomerRepository("PaySpan_JobSystem");
			this.companyName = RandomData.String(100);
			this.resellerId = 70000;
			this.address = RandomData.Int(100, 1000).ToString() + "Test St";
			this.city = "Jacksonville";
			this.zip = "32225";
			this.country = "US";
			this.state = "FL";
			this.fileTypeDescription = RandomData.String(50);
		}
	}
}