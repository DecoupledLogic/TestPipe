namespace TestPipe.Data.Dto.TestPipeRepo
{
	using System;
	using System.Collections.Generic;
	using NPoco;
	using TestPipe.Data.Transfer;

	[TableName("User")]
	[PrimaryKey("User_ID")]
	[ExplicitColumns]
	public partial class User : BaseDto
	{
		private string passwordHash;

		[Column]
		public bool Active { get; set; }

		[Column]
		public bool? Bulk_Email_Valid { get; set; }

		[Column]
		public DateTime? Creation_Date { get; set; }

		[Column]
		public string Digipass_Email_Address { get; set; }

		[Column]
		public int? Digipass_Notification_Option { get; set; }

		[Column]
		public string Email_Address1 { get; set; }

		[Column]
		public string Email_Address1_Style { get; set; }

		[Column]
		public string Email_Address2 { get; set; }

		[Column]
		public string Email_Address2_Style { get; set; }

		[Ignore]
		public string EmailAddress1Type { get; set; }

		[Ignore]
		public string EmailAddress2Type { get; set; }

		[Column]
		public int Failed_Logins { get; set; }

		[Column]
		public string Full_Name { get; set; }

		[Column]
		public bool Is_SystemUser { get; set; }

		[Column]
		public int? Items_Per_Page { get; set; }

		[Column]
		public DateTime? Last_Updated { get; set; }

		[Column]
		public string Last_Updated_By { get; set; }

		[Column]
		public string Mobile_Number { get; set; }

		[Column]
		public string Mobile_Operator_Code { get; set; }

		[Ignore]
		public string Mobile_Provider { get; set; }

		[Ignore]
		public string Password { get; set; }

		[Column]
		public bool Password_Expires_Next_Logon { get; set; }

		[Column]
		public string Password_Hash
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.passwordHash))
				{
					return this.passwordHash;
				}

				if (string.IsNullOrWhiteSpace(this.Password))
				{
					return string.Empty;
				}

				this.passwordHash = Hash.HashToString512(this.Password);
				return this.passwordHash;
			}
			set
			{
				this.passwordHash = value;
			}
		}

		[Column]
		public string Phone_Number { get; set; }

		[Ignore]
		public string PhoneExt { get; set; }

		[Ignore]
		public List<string> Roles { get; set; }

		[Column]
		public DateTime? Row_Created_Date { get; set; }

		[Column]
		public bool? Subscribe_To_Service { get; set; }

		[Ignore]
		public string Temp_Password_Datetime { get; set; }

		[Ignore]
		public string Temp_Password_Hash { get; set; }

		[Column]
		public int? Time_Zone_Id { get; set; }

		[Ignore]
		public string Title { get; set; }

		[Column]
		public string User_Challenge_Answer { get; set; }

		[Column]
		public string User_Challenge_Answer2 { get; set; }

		[Column]
		public int? User_Challenge_Question_ID { get; set; }

		[Column]
		public int? User_Challenge_Question_ID2 { get; set; }

		[Column]
		public int User_Group_ID { get; set; }

		[Column]
		public int User_ID { get; set; }

		[Column]
		public string User_Language { get; set; }

		[Column]
		public string User_Name { get; set; }

		[Column]
		public int? User_Tag_ID { get; set; }
	}
}