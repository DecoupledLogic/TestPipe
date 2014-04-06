namespace TestPipe.Specs
{
	using System;
	using System.Xml.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Core;
	using TestPipe.Data;
	using TestPipe.Data.Dto.TestPipeRepo;

	[TestClass]
	[DeploymentItem(@"Data\GenericDtoCacheSpec_User.xml")]
	public class GenericDtoCacheSpec
	{
		private User user;
		private XElement userElement;

		[TestMethod]
		public void RessedShouldReseedEntityInDatabase()
		{
			this.user = new User();
			this.user.Active = false;
			this.user.Failed_Logins = 0;
			this.user.Is_SystemUser = false;
			this.user.Password_Expires_Next_Logon = false;
			this.user.Password = "Abcd1234";
			this.user.User_Group_ID = 8379;
			this.user.User_ID = 0;
			this.user.User_Language = null;
			string userName = RandomData.String(8);
			this.user.User_Name = userName;

			string connectionConfig = "TestPipe";

			GenericRepository<User> repo = new GenericRepository<User>(connectionConfig);
			User newUser = repo.Insert(this.user);

			Assert.IsTrue(newUser.User_ID > 0, "newUser has invalid user ID.");

			this.user = repo.SelectSingle(newUser.User_ID);

			Assert.AreEqual(newUser.User_ID, this.user.User_ID, "Selected user doesn't match inserted user");

			User reseedUser = new User();
			reseedUser.Active = false;
			reseedUser.Failed_Logins = 0;
			reseedUser.Is_SystemUser = false;
			reseedUser.Password_Expires_Next_Logon = false;
			reseedUser.Password = "reseeded1234";
			reseedUser.User_Group_ID = 8379;
			reseedUser.User_ID = newUser.User_ID;
			reseedUser.User_Language = null;
			string newUserName = RandomData.String(8);
			reseedUser.User_Name = newUserName;

			GenericDtoCache.Reseed(reseedUser, connectionConfig);

			this.user = repo.SelectSingle(newUser.User_ID);

			Assert.AreEqual(newUser.User_ID, this.user.User_ID, "Selected user doesn't match inserted user");
			Assert.AreNotEqual(userName, this.user.User_Name);
		}

		[TestMethod]
		public void SeedShouldSeedEntityInDatabase()
		{
			this.user = new User();
			this.user.Active = false;
			this.user.Failed_Logins = 0;
			this.user.Is_SystemUser = false;
			this.user.Password_Expires_Next_Logon = false;
			this.user.Password = "Abcd1234";
			this.user.User_Group_ID = 8379;
			this.user.User_ID = 0;
			this.user.User_Language = null;
			string userName = RandomData.String(8);
			this.user.User_Name = userName;

			string connectionConfig = "TestPipe";

			User newUser = GenericDtoCache.Seed(this.user, connectionConfig);

			Assert.IsTrue(newUser.User_ID > 0, "newUser has invalid user ID.");

			GenericRepository<User> repo = new GenericRepository<User>(connectionConfig);
			this.user = repo.SelectSingle(newUser.User_ID);

			Assert.AreEqual(newUser.User_Name, this.user.User_Name, "Selected user doesn't match seeded user");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), "entity can't be a null value.")]
		public void SetPropertiesGivenNullEntityShouldThrowException()
		{
			this.user = null;
			this.userElement = XElement.Load("GenericDtoCacheSpec_User.xml");

			this.user = GenericDtoCache.SetProperties(this.userElement, this.user);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), "root can't be a null value.")]
		public void SetPropertiesGivenNullRootShouldThrowException()
		{
			this.user = new User();
			this.userElement = null;

			this.user = GenericDtoCache.SetProperties(this.userElement, this.user);
		}

		[TestMethod]
		public void SetPropertiesShouldSetEntityProperties()
		{
			this.user = new User();
			this.userElement = XElement.Load("GenericDtoCacheSpec_User.xml");

			this.user = GenericDtoCache.SetProperties(this.userElement, this.user);

			Assert.AreEqual(this.user.Active, false);
			Assert.AreEqual(this.user.Failed_Logins, 0);
			Assert.AreEqual(this.user.Is_SystemUser, false);
			Assert.AreEqual(this.user.Password_Expires_Next_Logon, false);
			Assert.AreEqual(this.user.Password, "Abcd1234");
			Assert.AreEqual(this.user.User_Group_ID, 8379);
			Assert.AreEqual(this.user.User_ID, 0);
			Assert.AreEqual(this.user.User_Language, null);
			Assert.AreEqual(this.user.User_Name, "testuser1");
		}

		[TestMethod]
		public void SetPropertyGivenEmptyElementShouldSetNullableEntityProperty()
		{
			this.user = new User();
			XElement userElement = XElement.Load("GenericDtoCacheSpec_User.xml");

			XElement element = userElement.Element("Items_Per_Page");

			GenericDtoCache.SetProperty(element, this.user);

			Assert.AreEqual(this.user.Items_Per_Page, null);
		}

		[TestMethod]
		public void SetPropertyGivenNullElementShouldNotSetEntityProperty()
		{
			this.user = new User();
			XElement element = null;

			GenericDtoCache.SetProperty(element, this.user);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetPropertyGivenNullEntityShouldThrowException()
		{
			this.user = null;
			this.userElement = XElement.Load("GenericDtoCacheSpec_User.xml");

			XElement element = this.userElement.Element("Password");

			GenericDtoCache.SetProperty(element, this.user);
		}

		[TestMethod]
		public void SetPropertyShouldSetEntityProperty()
		{
			this.user = new User();
			this.userElement = XElement.Load("GenericDtoCacheSpec_User.xml");

			XElement element = this.userElement.Element("Password");

			GenericDtoCache.SetProperty(element, this.user);

			Assert.AreEqual(this.user.Password, "Abcd1234");
		}
	}
}