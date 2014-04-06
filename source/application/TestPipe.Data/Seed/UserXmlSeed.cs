namespace TestPipe.Data.Seed
{
	using System;
	using System.Collections.Generic;
	using System.Xml.Linq;

	//<User>
	//	<User_Name>test</User_Name>
	//	<User_Group>testGroup</User_Group>
	//	<Tokens>
	//		<Token_ID>1</Token_ID>
	//		<Token_ID>2</Token_ID>
	//	</Tokens>
	//	<Roles>
	//		<Role_ID>4</Role_ID>
	//		<Role_ID>5</Role_ID>
	//	</Roles>
	//</User>
	public class UserXmlSeed : XmlSeedBase
	{
		//Unit test this crap
		private const string UserTag = "User";
		private const string UserNameTag = "User_Name";
		private const string UserGroupTag = "User_Group";
		private const string TokensTag = "Tokens";
		private const string TokenIdTag = "Token_ID";
		private const string RolesTag = "Roles";
		private const string RoleIdTag = "Role_ID";

		public UserXmlSeed()
		{
			this.RoleIds = new List<string>();
			this.TokenIds = new List<string>();
		}

		public ICollection<string> RoleIds { get; set; }

		public ICollection<string> TokenIds { get; set; }

		public string UserGroupId { get; set; }

		public string UserName { get; set; }

		public override void ProcessXml(XElement element)
		{
			if (element.Name.ToString() != UserTag)
			{
				return;
			}

			this.UserName = element.Element(UserNameTag).Value;
			this.UserGroupId = element.Element(UserGroupTag).Value;
			this.TokenIds = this.ProcessList(element, TokensTag, TokenIdTag);
			this.RoleIds = this.ProcessList(element, RolesTag, RoleIdTag);			
		}
	}
}