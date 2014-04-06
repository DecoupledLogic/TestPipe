namespace TestPipe
{
	using System;
	using System.Text;
	using System.Text.RegularExpressions;

	public class FormatValidator
	{
		/// <summary>
		/// Returns the country code component of an iban
		/// </summary>
		public static string CountryCodeFromIban(string iban)
		{
			return iban.Substring(0, 2);
		}

		public static bool IsNumeric(string s)
		{
			foreach (char c in s)
			{
				if (!char.IsNumber(c))
					return false;
			}
			return true;
		}

		public static bool IsValidAlphaNumericText(string alphaNum)
		{
			// Allows one or more alphabetical and/or numeric characters. This is a more generic
			// validation function.
			string regExPattern = @"^[A-Za-z0-9]+$";
			return MatchString(alphaNum, regExPattern);
		}

		public static bool IsValidAlphaText(string alpha)
		{
			// Allows one or more alphabetical characters. This is a more generic validation function.
			string regExPattern = @"^[A-Za-z]+$";
			return MatchString(alpha, regExPattern);
		}

		public static bool IsValidBermudaPostalCode(string code)
		{
			// The Bermuda postal code must be of the form AA<space>NN, AA<space>AA, AANN or AAAA where A
			// is any alphabetic character and N is a numeric digit from 0 to 9.
			// Matches: AA AA | AA 99 | AAAA
			// Non-Matches: 99 AA | A09 | 1234 | AAA 999
			string regExPattern = @"^([A-Z]{2}[\s]|[A-Z]{2})[\w]{2}$";
			return MatchString(code, regExPattern);
		}

		/// <summary>
		/// Performs patternmatching ont he input string to ensure it is in a valid BIC format.
		/// </summary>
		/// <param name="countryCode">For a valid BIC code, will be set to the country code</param>
		/// <remarks>
		/// Does not validate that the country code part of the input is a valid ISO country code
		/// </remarks>
		public static bool IsValidBIC(string input, out string countryCode)
		{
			if (input == null)
				throw new ArgumentException("input must not be null");

			countryCode = null;

			// Check the length
			if (input.Length < 8 || input.Length > 12)
				return false;

			// Build a Regular expression to validate the mandatory part of the BIC (first 8 characters)
			StringBuilder regex = new StringBuilder("^");

			// Characters 1-6: Letters Only
			regex.Append("[A-Z]{6}");

			// Character 7: Region code, one alpha numeric character, digits 0 and 1 not permitted
			regex.Append("[A-Z2-9]{1}");

			// Character 8: Location suffix code, one alpha numeric character, 0 and o not permitted
			regex.Append("[A-NP-Z1-9]{1}");

			if (!Regex.IsMatch(input, regex.ToString(), RegexOptions.IgnoreCase))
				return false;

			// Test the optional part of the BIC
			if (input.Length > 8)
			{
				string optional = input.Substring(8).ToUpper();

				// Character 9: Location code, one alpha numeric character, digits 0 and 1 not permitted
				// Characters 10-12: Branch code, 1-3 alpha numeric
				if (!Regex.IsMatch(optional, "^[A-Z2-9]{1}[A-Z\\d]{0,3}"))
					return false;

				// if the branch code is present, it may only start with 'X' if it's "XXX"
				if (optional.Length > 1 && optional[1] == 'X' && !optional.EndsWith("XXX"))
					return false;

				// Branch code may not be "BIC"
				if (optional.EndsWith("BIC"))
					return false;
			}

			countryCode = input.Substring(4, 2);
			return true;
		}

		public static bool IsValidCanadianPostalCode(string code)
		{
			// The Canadian postal code must be of the form ANA<space>NAN where A is any alphabetic
			// character and N is a numeric digit from 0 to 9.
			string regExPattern = @"^[a-zA-Z][0-9][a-zA-Z][ ]?[0-9][a-zA-Z][0-9]$";
			return MatchString(code, regExPattern);
		}

		public static bool IsValidCCNumber(string number)
		{
			// This expression is basically looking for series of numbers confirming to the standards for
			// Visa, MC, Discover and American Express with optional dashes between groups of numbers
			string regExPattern = @"^((4\d{3})|(5[1-5]\d{2})|(6011))-?\d{4}-?\d{4}-?\d{4}|3[4,7][\d\s-]{15}$";
			if (!MatchString(number, regExPattern))
				return false;

			//check for null
			if (number == null)
				return true;

			// Apply check digit logic to confirm credit card number is valid

			//Replace any character other than 0-9 with ""
			number = Regex.Replace(number, @"[^0-9]", string.Empty);

			int cardSize = number.Length;

			//Creditcard number length must be between 13 and 16
			if (cardSize >= 13 && cardSize <= 16)
			{
				int odd = 0;
				int even = 0;
				char[] cardNumberArray = new char[cardSize];

				//Read the creditcard number into an array
				cardNumberArray = number.ToCharArray();

				//Reverse the array
				Array.Reverse(cardNumberArray, 0, cardSize);

				//Multiply every second number by two and get the sum.
				//Get the sum of the rest of the numbers.
				for (int i = 0; i < cardSize; i++)
				{
					if (i % 2 == 0)
						odd += Convert.ToInt32(cardNumberArray.GetValue(i)) - 48;
					else
					{
						int temp = (Convert.ToInt32(cardNumberArray[i]) - 48) * 2;

						//if the value is greater than 9, substract 9 from the value
						if (temp > 9)
							temp = temp - 9;

						even += temp;
					}
				}
				if ((odd + even) % 10 == 0)
					return true;
				else
					return false;
			}
			else
				return false;
		}

		public static bool IsValidCHIPSNumber(string chipsNumber)
		{
			// 4 or 6
			string regExPattern = @"[a-zA-Z0-9]{6}$";
			if (MatchString(chipsNumber, regExPattern))
				return true;

			regExPattern = @"[a-zA-Z0-9]{4}$";
			if (MatchString(chipsNumber, regExPattern))
				return true;

			return false;
		}

		public static bool IsValidEmailAddress(string email)
		{
			// Allows common email address that can start with a alphanumeric char and contain word, dash
			// and period characters followed by a domain name meeting the same criteria followed by a
			// alpha suffix between 2 and 9 character lone
			string regExPattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
			return MatchString(email, regExPattern);
		}

		/// <summary>
		/// Performs the preparatory string manipulation and the mod 97 validationfor a IBAN. Does not
		/// validate the length or the country code.
		/// </summary>
		public static bool IsValidIBAN(string input)
		{
			// Make sure the input is alphanumeric
			if (!FormatValidator.IsValidAlphaNumericText(input))
				return false;

			// Move the first 4 characters to the end of the string
			StringBuilder sb = new StringBuilder(input.Substring(4).ToUpper());
			sb.Append(input.Substring(0, 4));

			// Replace each letter in the string with two digits, thereby expanding the string, where
			// A=10, B=11, ..., Z=35
			StringBuilder sb2 = new StringBuilder();
			for (int i = 0; i < sb.Length; i++)
			{
				int ascii = (int)sb[i];
				if (ascii >= 65)
					sb2.Append((ascii - 55).ToString());
				else
					sb2.Append(sb[i]);
			}

			string bigInt = sb2.ToString();

			// Perform mod 97 on the integer value of bigInt. This is too large for an Int64 so we have to
			// use a different technique
			int mod = 0;
			while (true)
			{
				int chunkLength = bigInt.Length > 16 ? 16 : bigInt.Length;
				long tmp = long.Parse(bigInt.Substring(0, chunkLength));
				mod = (int)(tmp % 97);
				bigInt = string.Format("{0}{1}", mod, bigInt.Substring(chunkLength));
				if (chunkLength < 16)
					break;
			}
			return mod == 1;
		}

		public static bool IsValidIPAddress(string ip)
		{
			// Allows four octets of numbers that contain values between 4 numbers in the IP address to
			// 0-255 and are separated by periods
			string regExPattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
			return MatchString(ip, regExPattern);
		}

		public static bool IsValidNumericText(string numeric)
		{
			// Allows one or more positive or negative, integer or decimal numbers. This is a more generic
			// validation function.
			string regExPattern = @"/[+-]?\d+(\.\d+)?$";
			return MatchString(numeric, regExPattern);
		}

		public static bool IsValidSSN(string ssn)
		{
			// Allows SSN's of the format 123-456-7890. Accepts hyphen delimited SSN’s or plain numeric values.
			string regExPattern = @"^\d{3}[-]?\d{2}[-]?\d{4}$";
			return MatchString(ssn, regExPattern);
		}

		public static bool IsValidSSOUserName(string userName)
		{
			// sso usernames are a max of 50 chars long and have format of ~bankparnter~username
			string regExPattern = @"~[a-zA-Z0-9]{1,23}~.{1,1000}$";
			return MatchString(userName, regExPattern);
		}

		public static bool IsValidSwiftNumber(string swiftNumber)
		{
			//8 or 11 characters in length and alpha numberic only
			string regExPattern = @"[a-zA-Z0-9]{8}$";
			if (MatchString(swiftNumber, regExPattern))
				return true;

			regExPattern = @"[a-zA-Z0-9]{11}$";
			if (MatchString(swiftNumber, regExPattern))
				return true;

			return false;
		}

		public static bool IsValidURL(string url)
		{
			// Allows HTTP and FTP URL's, domain name must start with alphanumeric and can contain a port
			// number followed by a path containing a standard path character and ending in common file
			// suffixies found in URL's and accounting for potential CGI GET data
			string regExPattern = @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";
			return MatchString(url, regExPattern);
		}

		public static bool IsValidURLRelaxed(string url)
		{
			// This method is different from the one above in so far as it does not require the HTTP:// or
			// the FTP://
			string regExPattern = @"^^((ht|f)tp(s?)\:\/\/)?[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";
			return MatchString(url, regExPattern);
		}

		public static bool IsValidUSPhoneNumber(string phone)
		{
			// Allows phone number of the format: NPA = [2-9][0-8][0-9] Nxx = [2-9][0-9][0-9] Station = [0-9][0-9][0-9][0-9]
			string regExPattern = @"^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$";
			return MatchString(phone, regExPattern);
		}

		public static bool IsValidZIPCode(string zip)
		{
			// Allows 5 digit, 5+4 digit and 9 digit zip codes must be at least two characters long and
			// caps out at 128 (database size)
			string regExPattern = @"^(\d{5}-\d{4}|\d{5}|\d{9})$";
			return MatchString(zip, regExPattern);
		}

		// function designed to take a string and regular expression and return true if the regex
		// validates the string false if the string fails the regex.
		public static bool MatchString(string str, string regexstr)
		{
			str = str.Trim();
			Regex pattern = new Regex(regexstr);
			return pattern.IsMatch(str);
		}

		public static object ValidateDataType(string fieldType, ref string s)
		{
			// Trim non-string types
			if (fieldType != "string")
				s = s.Trim();

			// Validate based on data type
			switch (fieldType.ToLower())
			{
				case "float":
				case "money":
				case "decimal":
					return ValidateDecimal(ref s);

				case "bigint":
					return ValidateInt64(ref s);

				case "int":
					return ValidateInt(ref s);

				case "datetime":
					return ValidateDateTime(ref s);

				case "string":
					return s;

				default:
					throw new InvalidOperationException("Invalid field type: " + fieldType);
			}
		}

		public static DateTime ValidateDateTime(ref string s)
		{
			// Support yyyyMMdd style format
			if (IsNumeric(s) && s.Length == 8)
			{
				int year, mon, day;
				try
				{
					year = int.Parse(s.Substring(0, 4));
					mon = int.Parse(s.Substring(4, 2));
					day = int.Parse(s.Substring(6, 2));
				}
				catch
				{
					throw new FormatException("Invalid date format:" + s);
				}
				s = string.Format("{0}/{1}/{2}", mon, day, year);
			}
			return Convert.ToDateTime(s);
		}

		public static decimal ValidateDecimal(ref string s)
		{
			// Handle (123.00) as a negative #
			if (s.StartsWith("(") && s.EndsWith(")"))
				s = "-" + s.Substring(1, s.Length - 2).Trim();

			// Handle $xx.yy
			if (s.StartsWith("$"))
				s = s.Substring(1).Trim();

			// Remove spaces
			s = s.Replace(" ", string.Empty);
			return Convert.ToDecimal(s);
		}

		public static int ValidateInt(ref string s)
		{
			// Handle x.yy as a valid integer when yy is zero
			string[] parts = s.Split('.');
			if (parts.Length == 2 && Convert.ToInt32(parts[1]) == 0)
				s = parts[0];

			// Remove spaces
			s = s.Replace(" ", string.Empty);
			return Convert.ToInt32(s);
		}

		public static long ValidateInt64(ref string s)
		{
			// Handle x.yy as a valid integer when yy is zero
			string[] parts = s.Split('.');
			if (parts.Length == 2 && Convert.ToInt32(parts[1]) == 0)
				s = parts[0];

			// Remove spaces
			s = s.Replace(" ", string.Empty);
			return Convert.ToInt64(s);
		}
	}
}