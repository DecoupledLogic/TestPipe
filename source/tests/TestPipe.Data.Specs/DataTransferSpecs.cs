namespace TestPipe.Data.JobSystem.Test
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Xml;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Data.Deserializers;
	using TestPipe.Data.Security;
	using TestPipe.Data.Serializers;
	using TestPipe.Data.Transfer.Dto.JobSystem;
	using TestPipe.Data.Transfer.Dto.Security;

	[TestClass]
	[DeploymentItem(@"Data\TestData.xml")]

	//[DeploymentItem(@"Data\UserAdministration.xml")]
	public class DataTransferSpecs
	{
		[TestMethod]
		public void DB_To_DTO()
		{
			CustomerRepository repo = new CustomerRepository("PaySpan_JobSystem");

			Customer dto = new Customer();
			dto.Company_Name = "Test Me";

			repo.AddCustomer(dto);

			Console.WriteLine("Start DB_To_DTO");
			Stopwatch sw = new Stopwatch();
			sw.Start();
			Customer newDto = repo.GetCustomer(dto.Customer_ID);
			sw.Stop();
			Console.WriteLine("Time: " + sw.Elapsed);

			Assert.AreEqual(dto.Customer_ID, newDto.Customer_ID);
		}

		[TestMethod]
		public void DB_To_User_DTO()
		{
			UserRepository repo = new UserRepository("PaySpan_Security");

			User dto = new User();
			dto.User_Group_ID = 8379;
			dto.User_Name = RandomData.String(8);
			dto.Password_Expires_Next_Logon = false;
			dto.Failed_Logins = 0;
			dto.Is_SystemUser = false;

			repo.InsertUser(dto);

			User newDto = repo.SelectUser(dto.User_ID);

			Assert.AreEqual(dto.User_ID, newDto.User_ID);
		}

		[TestMethod]
		public void DB_To_User_Group_DTO()
		{
			UserRepository repo = new UserRepository("PaySpan_Security");

			User_Group dto = new User_Group();
			dto.Group_Name = RandomData.String(8);
			dto.User_Group_Type_ID = 1;

			repo.InsertUser_Group(dto);

			User_Group newDto = repo.SelectUser_Group(dto.User_Group_ID);

			Assert.AreEqual(dto.User_Group_ID, newDto.User_Group_ID);
		}

		[TestMethod]
		public void DTO_To_DB()
		{
			CustomerRepository repo = new CustomerRepository("PaySpan_JobSystem");

			Customer dto = new Customer();
			dto.Company_Name = "Test Me";

			repo.AddCustomer(dto);

			Customer newDto = repo.GetCustomer(dto.Customer_ID);

			Assert.AreEqual(dto.Customer_ID, newDto.Customer_ID);
		}

		[TestMethod]
		public void DTO_To_XML()
		{
			Customer dto = new Customer();
			dto.Customer_ID = 1000;

			XmlObjectSerializer serializer = new XmlObjectSerializer();

			//string xml = serializer.Serialize(dto, dto.GetType().Name);
		}

		[TestMethod]
		public void Load_XML_To_DTO()
		{
			string path = "TestData.xml";

			if (!File.Exists(path))
			{
				throw new AssertFailedException("file not found.");
			}

			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			string xmlcontents = doc.InnerXml;

			XmlNodeList nodeList = doc.SelectNodes("TestData");
			XmlNode root = nodeList[0];

			IList<Customer> list = new List<Customer>();

			foreach (XmlNode xnode in root.ChildNodes)
			{
				XmlObjectDeserializer deserializer = new XmlObjectDeserializer();

				Customer dto = deserializer.Deserialize<Customer>(xnode.OuterXml, new Customer());

				list.Add(dto);
			}

			Assert.AreEqual(2, list.Count);
		}

		[TestMethod]
		public void User_DTO_To_XML()
		{
			User dto = new User();
			dto.User_Group_ID = 8379;
			dto.User_Name = "testuser1";
			dto.Password_Expires_Next_Logon = false;
			dto.Failed_Logins = 0;
			dto.Is_SystemUser = false;

			XmlObjectSerializer serializer = new XmlObjectSerializer();

			string xml = serializer.Serialize(dto, "Security." + dto.GetType().Name);
		}

		[TestMethod]
		public void XML_To_DTO()
		{
			XmlObjectDeserializer deserializer = new XmlObjectDeserializer();

			Console.WriteLine("Start XML_To_DTO");
			Stopwatch sw = new Stopwatch();
			sw.Start();
			Customer newDto = deserializer.Deserialize<Customer>(this.GetXmlCustomer(), new Customer());
			sw.Stop();
			Console.WriteLine("Time: " + sw.Elapsed);

			Assert.AreEqual(1000, newDto.Customer_ID);
		}

		private string GetDtoToXml()
		{
			return "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Customer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Customer_ID>1000</Customer_ID>\r\n  <Default_Billing_Account_ID xsi:nil=\"true\" />\r\n  <Enable_Penny_Deposits xsi:nil=\"true\" />\r\n  <Inactive xsi:nil=\"true\" />\r\n  <Individual xsi:nil=\"true\" />\r\n  <Last_Modified_On xsi:nil=\"true\" />\r\n  <Marketing_Opt_In xsi:nil=\"true\" />\r\n  <Parent_Customer_ID xsi:nil=\"true\" />\r\n  <Reseller_ID xsi:nil=\"true\" />\r\n  <Row_Created_Date xsi:nil=\"true\" />\r\n  <Type_ID xsi:nil=\"true\" />\r\n</Customer>";
		}

		private string GetXmlCustomer()
		{
			return "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Customer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Customer_ID>1000</Customer_ID>\r\n  <Parent_Customer_ID xsi:nil=\"true\" />\r\n  <Company_Name>Westdale Wireless</Company_Name>\r\n  <Reseller_ID>70000</Reseller_ID>\r\n  <Address_1>123 Main St</Address_1>\r\n  <Address_2 />\r\n  <Address_3 />\r\n  <City>Jacksonville</City>\r\n  <Zip>32256</Zip>\r\n  <Country>United States of America</Country>\r\n  <Comments />\r\n  <Inactive>false</Inactive>\r\n  <Row_Created_Date>2013-05-15T15:00:37.96</Row_Created_Date>\r\n  <State>FL</State>\r\n  <Type_ID>1</Type_ID>\r\n  <URL />\r\n  <Last_Modified_By>jmsuser</Last_Modified_By>\r\n  <Last_Modified_On>2013-05-15T15:00:37.96</Last_Modified_On>\r\n  <Tax_ID>1</Tax_ID>\r\n  <Marketing_Opt_In xsi:nil=\"true\" />\r\n  <Individual>false</Individual>\r\n  <vCard_Account_Code />\r\n  <Default_Billing_Account_ID xsi:nil=\"true\" />\r\n  <Address_Code />\r\n  <Tax_Class />\r\n  <Tax_Code />\r\n  <Location_Code />\r\n  <Ship_Address_Code />\r\n  <Ship_Tax_Class />\r\n  <Ship_Tax_Code />\r\n  <Ship_Location_Code />\r\n  <Exempt_Tax_Code />\r\n  <Ship_Exempt_Tax_Code />\r\n  <AccountingPackage />\r\n  <ERPsystemType />\r\n  <Enable_Penny_Deposits xsi:nil=\"true\" />\r\n  <BankCustomer_ID />\r\n  <Customer_SF_ID />\r\n</Customer>";
		}
	}
}