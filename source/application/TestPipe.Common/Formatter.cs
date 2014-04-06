namespace TestPipe
{
	using System;
	using System.Text;

	public class Formater
	{
		public static string ByteToHexString(byte[] inBytes)
		{
			string stringOut = string.Empty;
			foreach (byte inByte in inBytes)
				stringOut = stringOut + string.Format("{0:X2}", inByte);

			return stringOut;
		}

		public static string Capitalize(string inputString)
		{
			if (string.IsNullOrEmpty(inputString))
				return string.Empty;

			string[] tokenString = inputString.Trim().Split(new char[] { ' ' });
			int i = 0;
			foreach (string word in tokenString)
			{
				if (!string.IsNullOrEmpty(word))
					tokenString[i] = word.Substring(0, 1).ToUpper() + word.Remove(0, 1).ToLower();
				else
					tokenString[i] = word;
				i++;
			}
			return string.Join(" ", tokenString);
		}

		public static string FormatCreditCard(string maskedCard)
		{
			if (string.IsNullOrEmpty(maskedCard))
				return maskedCard;

			maskedCard = maskedCard.Replace("-", string.Empty);
			maskedCard = maskedCard.Replace(" ", string.Empty);
			StringBuilder card = new StringBuilder(maskedCard, 20);
			return card.Insert(12, '-').Insert(8, '-').Insert(4, '-').ToString();
		}

		public static string FormatMobileNumber(string mobileNumber)
		{
			if (mobileNumber.Length == 10)
				return string.Format("({0}) {1}-{2}", mobileNumber.Substring(0, 3), mobileNumber.Substring(3, 3), mobileNumber.Substring(6, 4));
			else
				return mobileNumber;
		}

		public static string FormatUserNameForViewing(string userName)
		{
			if (!string.IsNullOrWhiteSpace(userName) && FormatValidator.IsValidSSOUserName(userName))
			{
				int i = userName.LastIndexOf('~');
				return userName.Substring(i + 1, userName.Length - i - 1);
			}
			return userName;
		}

		public static string PhoneSplit(string strPhoneNumber, string strPhoneNumberSegment)
		{
			switch (strPhoneNumberSegment)
			{
				case "AreaCode":
					return strPhoneNumber.Substring(0, 3);

				case "Prefix":
					return strPhoneNumber.Substring(3, 3);

				case "Base":
					return strPhoneNumber.Substring(6, 4);

				case "Ext":
					return strPhoneNumber.Substring(10, strPhoneNumber.Length - 10);

				default:
					return strPhoneNumber;
			}
		}

		public static string XReplace(string s, int leaveVisible, char replaceChar)
		{
			char[] x = new char[s.Length - leaveVisible];
			for (int i = 0; i < x.Length; i++)
				x[i] = replaceChar;

			return new string(x) + s.Substring(s.Length - leaveVisible, leaveVisible);
		}

		public static string GetElapsedTime(TimeSpan ts)
		{
			string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
			return elapsedTime;
		}
	}
}