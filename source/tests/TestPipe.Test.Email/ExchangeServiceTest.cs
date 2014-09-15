﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestPipe.Email;

namespace TestPipe.Test.Email
{
	[TestClass]
	public class ExchangeServiceTest
	{
		[TestMethod]
		public void Count_Unread()
		{
			
			ExchangeService svc = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
			svc.Url = new Uri("https://voo-casarray01.internal.sungard.corp/EWS/Exchange.asmx");
			MailBox sut = new MailBox(svc);

			FolderView viewFolders = new FolderView(int.MaxValue) { Traversal = FolderTraversal.Deep, PropertySet = new PropertySet(BasePropertySet.IdOnly) };
			ItemView viewEmails = new ItemView(int.MaxValue) { PropertySet = new PropertySet(BasePropertySet.IdOnly) };
			SearchFilter unreadFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false));
			SearchFilter folderFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, new SearchFilter.IsEqualTo(FolderSchema.DisplayName, "AllItems"));

			FindFoldersResults inboxFolders = svc.FindFolders(WellKnownFolderName.Root, folderFilter, viewFolders);
			int unreadCount = 0;

			//search all items in Inbox and subfolders
			FindItemsResults<Item> findResults = svc.FindItems(WellKnownFolderName.Inbox, unreadFilter, viewEmails);
			unreadCount += findResults.TotalCount;

			inboxFolders = svc.FindFolders(WellKnownFolderName.Inbox, viewFolders);
			foreach (Folder folder in inboxFolders.Folders)
			{
				findResults = svc.FindItems(folder.Id, unreadFilter, viewEmails);
				unreadCount += findResults.TotalCount;
			}
		}

		[TestMethod]
		public void Find_Message()
		{

			ExchangeService svc = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
			svc.Url = new Uri("https://voo-casarray01.internal.sungard.corp/EWS/Exchange.asmx");
			MailBox sut = new MailBox(svc);

			ItemView viewEmails = new ItemView(int.MaxValue) { PropertySet = new PropertySet(BasePropertySet.IdOnly) };
			SearchFilter filter = new SearchFilter.ContainsSubstring(ItemSchema.Subject, "Puppet");

			FindItemsResults<Item> findResults = svc.FindItems(WellKnownFolderName.Inbox, filter, viewEmails);
			svc.LoadPropertiesForItems(findResults.Items, PropertySet.FirstClassProperties);
			
			string result = string.Empty;
			
			if (findResults.Items.Count > 0) // Prevent the exception -- "You must load or assign this property before you can read its value."
			{
				foreach (Item item in findResults)
				{
					result = item.Body;
				}
			}

			Assert.IsTrue(result.Length > 0);
		}

		[TestMethod]
		public void Remove_Line_With_Breaks()
		{
			string text = "Address1<br>\n\rCity, State Zip<br>";

			string actual = Regex.Replace(text, @"\bAddress1<br>", "");

			Assert.AreEqual("\n\rCity, State Zip<br>", actual);
		}

		[TestMethod]
		public void Remove_Line_With_Breaks_From_File()
		{
			string input = GetOriginal();

			//using (StreamReader sr = new StreamReader(@"C:\CustomerEmail.txt"))
			//{
			//	input = sr.ReadToEnd();
			//}

			string actual = Regex.Replace(input, @"Address1<br>", "");

			Assert.AreEqual(GetResult(), actual);
		}

		public string GetOriginal()
		{
			return "<html>\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n<title>PayNetExchange Payment Notification</title>\r\n<style>\r\n\t\t\t\t\t\t\tbody \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tfont-family: 'Arial';\r\n\t\t\t\t\t\t\t\tfont-size: 12px;\r\n\t\t\t\t\t\t\t\tcolor: #666666;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\ta \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tcolor: #014F5B;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\th1, h2 \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tfont-family: 'Arial';\r\n\t\t\t\t\t\t\t\ttext-transform:uppercase;\r\n\t\t\t\t\t\t\t\tcolor: #014F5B;\r\n\t\t\t\t\t\t\t\tfont-weight: bold;\r\n\t\t\t\t\t\t\t\tfont-size: 14px;\r\n\t\t\t\t\t\t\t\tfont-style: italic;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.headRow\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tfont-weight:bold;\r\n\t\t\t\t\t\t\t\tcolor:#666666;\r\n\t\t\t\t\t\t\t\tpadding:2px 12px 2px 0px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t#splitter \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tborder: solid 1px #dddddd;\r\n\t\t\t\t\t\t\t\tmargin-left: 24px;\r\n\t\t\t\t\t\t\t\tmargin-right: 24px;\r\n\t\t\t\t\t\t\t}\r\n\t\t\t\t\t\t\t.mail\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\twidth: 700px;\r\n\t\t\t\t\t\t\t\tcolor: #666666;\r\n\t\t\t\t\t\t\t\tpadding: 0px;\r\n\t\t\t\t\t\t\t\tmargin: 0px;\r\n\t\t\t\t\t\t\t\tbackground-color:#ffffff;\r\n\t\t\t\t\t\t\t\tborder:solid 1px #999999;\r\n\t\t\t\t\t\t\t}\r\n\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t#mailWrapper \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tborder: solid 1px #dddddd;\r\n\t\t\t\t\t\t\t\twidth: 700px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t#mailBody \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tpadding-left: 32px;\r\n\t\t\t\t\t\t\t\tpadding-bottom: 32px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.mailBody\r\n\t\t\t\t\t\t\t{\t\r\n\t\t\t\t\t\t\t\tcolor: #666666;\r\n\t\t\t\t\t\t\t\tfont-family: Arial;\r\n\t\t\t\t\t\t\t\tfont-size: 12px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.footerTable\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tpadding-top: 10px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t#footerLogo \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tpadding: 24px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t#footerText p\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tpadding: 0;\r\n\t\t\t\t\t\t\t\tmargin: 0;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t#footerText \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tpadding-left: 24px;\r\n\t\t\t\t\t\t\t\tfont-size: 10px;\r\n\t\t\t\t\t\t\t\tcolor: #666666;\r\n\t\t\t\t\t\t\t}\r\n\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t.error {\r\n\t\t\t\t\t\t\t\tfont-weight: bold;\r\n\t\t\t\t\t\t\t\tcolor: red;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.gridHeading\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t  background-color:#A3A29D;\r\n\t\t\t\t\t\t\t  padding:10px 0px 10px 5px;\r\n\t\t\t\t\t\t\t  color:#FFFFFF;\r\n\t\t\t\t\t\t\t  font-weight:bold;\r\n\t\t\t\t\t\t\t  font-family:Arial;\r\n\t\t\t\t\t\t\t  font-size:12px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.gridHeaderRow\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t  background-color:#F2F2F4;\r\n\t\t\t\t\t\t\t  color:#666666;\r\n\t\t\t\t\t\t\t  padding:4px;\r\n\t\t\t\t\t\t\t  font-family: Arial;\r\n\t\t\t\t\t\t\t  font-size: 11px;\r\n\t\t\t\t\t\t\t  font-weight:bold;\r\n\t\t\t\t\t\t\t  border-bottom:solid 1px #E4E4E4;\r\n\t\t\t\t\t\t\t  padding: 0px 10px 0px 10px;\r\n\t\t\t\t\t\t\t  height:25px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.gridRow\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t  background-color:#FFFFFF;\r\n\t\t\t\t\t\t\t  color:#666666;\r\n\t\t\t\t\t\t\t  border-bottom:solid 1px #E4E4E4;\r\n\t\t\t\t\t\t\t  padding:4px;\r\n\t\t\t\t\t\t\t  font-family: Arial;\r\n\t\t\t\t\t\t\t  padding: 0px 10px 0px 10px;\r\n\t\t\t\t\t\t\t  font-size: 11px;\r\n\t\t\t\t\t\t\t  height:25px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.step-row\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t  padding: 0;\r\n\t\t\t\t\t\t\t  height: 70px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.step-icon\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t  padding-left:16px;\r\n\t\t\t\t\t\t\t  padding-right:16px;\r\n\t\t\t\t\t\t\t  text-align:center;\r\n\t\t\t\t\t\t\t}\r\n\t\t\t\t\t\t</style>\r\n</head>\r\n<body>\r\n<table id=\"mailWrapper\" cellpadding=\"0\" cellspacing=\"0\">\r\n<tbody>\r\n<tr>\r\n<td colspan=\"2\">\r\n<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n<tbody>\r\n<tr>\r\n<td style=\"background-color:#000000;height:96px;padding-left: 4px;padding-right: 4px;margin:0px;\" valign=\"center\">\r\n<img alt=\"header\" style=\"border:0;padding:0px; margin:0px;display:block;\" src=\"https://www.paynetexchange.com/sungard/payments/images/sungard/\\sungard.gif\r\n\t\t\t\t\t\t\t\t\t\t\t\"></td>\r\n<td style=\"background-color:#014F5B;padding-left:4px;\" width=\"100%\"><span style=\"font-family:Arial; font-weight:bold; font-size:17px; color:#FFFFFF;padding-left:4px;\">PayNetExchange</span></td>\r\n<td><img alt=\"header\" style=\"border:0;padding:0px; margin:0px;display:block;\" src=\"https://www.paynetexchange.com/sungard/payments/images/sungard/\\bg-header.gif\r\n\t\t\t\t\t\t\t\t\t\t\t\"></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<br>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td id=\"mailBody\"><br>\r\n<br>\r\n<h1>Card Blocked</h1>\r\n<p>A credit card payment has been made by <strong>The Feast</strong> to <strong>LARS ULRICH.\r\n</strong>has been blocked.<br>\r\n<br>\r\n</p>\r\n<p class=\"error\">This card may no longer be processed. </p>\r\n<table cellpadding=\"0\" cellspacing=\"0\">\r\n<tbody>\r\n<tr>\r\n<td style=\"text-align: right; width: 150px; padding-right: 8px;\">Amount: </td>\r\n<td>$1000</td>\r\n</tr>\r\n<tr>\r\n<td style=\"text-align: right; width: 150px; padding-right: 8px;\">Payment Number: </td>\r\n<td>6001109121</td>\r\n</tr>\r\n<tr>\r\n<td style=\"text-align: right; width: 150px; padding-right: 8px;\">Account Number: <br>\r\n<br>\r\n</td>\r\n<td>IDDQD<br>\r\n<br>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td style=\"text-align: right; width: 150px; padding-right: 8px;\" valign=\"top\">Payer:\r\n</td>\r\n<td>The Feast<br>\r\nAddress1<br>\r\nMantoza, FL 32206<br>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n<td valign=\"top\" align=\"right\"></td>\r\n</tr>\r\n<tr>\r\n<td colspan=\"2\" style=\"padding-left: 32px; padding-right: 15px;\">\r\n<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n<tbody>\r\n<tr>\r\n<td width=\"16%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"left\">Invoice Number</td>\r\n<td width=\"13%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"left\">Invoice Date</td>\r\n<td width=\"30%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"left\">Description</td>\r\n<td width=\"15%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"right\">Gross Amount</td>\r\n<td width=\"13%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"right\">Discount</td>\r\n<td width=\"13%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"right\">Net Amount</td>\r\n</tr>\r\n<tr>\r\n<td valign=\"middle\" width=\"16%\" class=\"gridRow\" align=\"left\">9999000111</td>\r\n<td valign=\"middle\" width=\"13%\" class=\"gridRow\" align=\"left\">08-29-2009</td>\r\n<td valign=\"middle\" width=\"30%\" class=\"gridRow\" align=\"left\">Group Management</td>\r\n<td valign=\"middle\" width=\"15%\" class=\"gridRow\" align=\"right\">$1,000.00</td>\r\n<td valign=\"middle\" width=\"13%\" class=\"gridRow\" align=\"right\">$0.00</td>\r\n<td valign=\"middle\" width=\"13%\" class=\"gridRow\" align=\"right\">$1,000.00</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td colspan=\"2\" style=\"text-align: center;\"><br>\r\n<br>\r\n<hr style=\"height: 1px; width: 670px;\">\r\n</td>\r\n</tr>\r\n<tr>\r\n<td class=\"mailBody\" colspan=\"2\">\r\n<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" class=\"footerTable\">\r\n<tbody>\r\n<tr>\r\n<td id=\"footerText\">© 2014 SunGard\r\n<p>Trademark information: SunGard and the SunGard logo are trademarks or registered trademarks of SunGard Data Systems Inc. or its subsidiaries in the U.S. and other countries. All other trade names are trademarks or registered trademarks of their respective\r\n holders. </p>\r\n</td>\r\n<td id=\"footerLogo\"><img alt=\"SunGard PayNetExchange\" src=\"https://www.paynetexchange.com/sungard/payments/images/sungard/\\thinkbeforeprint.jpg\r\n\t\t\t\t\t\t\t\t\t\t\t\t\"></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</body>\r\n</html>\r\n";
		}

		public string GetResult()
		{
			return "<html>\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n<title>PayNetExchange Payment Notification</title>\r\n<style>\r\n\t\t\t\t\t\t\tbody \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tfont-family: 'Arial';\r\n\t\t\t\t\t\t\t\tfont-size: 12px;\r\n\t\t\t\t\t\t\t\tcolor: #666666;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\ta \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tcolor: #014F5B;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\th1, h2 \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tfont-family: 'Arial';\r\n\t\t\t\t\t\t\t\ttext-transform:uppercase;\r\n\t\t\t\t\t\t\t\tcolor: #014F5B;\r\n\t\t\t\t\t\t\t\tfont-weight: bold;\r\n\t\t\t\t\t\t\t\tfont-size: 14px;\r\n\t\t\t\t\t\t\t\tfont-style: italic;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.headRow\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tfont-weight:bold;\r\n\t\t\t\t\t\t\t\tcolor:#666666;\r\n\t\t\t\t\t\t\t\tpadding:2px 12px 2px 0px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t#splitter \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tborder: solid 1px #dddddd;\r\n\t\t\t\t\t\t\t\tmargin-left: 24px;\r\n\t\t\t\t\t\t\t\tmargin-right: 24px;\r\n\t\t\t\t\t\t\t}\r\n\t\t\t\t\t\t\t.mail\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\twidth: 700px;\r\n\t\t\t\t\t\t\t\tcolor: #666666;\r\n\t\t\t\t\t\t\t\tpadding: 0px;\r\n\t\t\t\t\t\t\t\tmargin: 0px;\r\n\t\t\t\t\t\t\t\tbackground-color:#ffffff;\r\n\t\t\t\t\t\t\t\tborder:solid 1px #999999;\r\n\t\t\t\t\t\t\t}\r\n\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t#mailWrapper \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tborder: solid 1px #dddddd;\r\n\t\t\t\t\t\t\t\twidth: 700px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t#mailBody \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tpadding-left: 32px;\r\n\t\t\t\t\t\t\t\tpadding-bottom: 32px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.mailBody\r\n\t\t\t\t\t\t\t{\t\r\n\t\t\t\t\t\t\t\tcolor: #666666;\r\n\t\t\t\t\t\t\t\tfont-family: Arial;\r\n\t\t\t\t\t\t\t\tfont-size: 12px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.footerTable\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tpadding-top: 10px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t#footerLogo \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tpadding: 24px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t#footerText p\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tpadding: 0;\r\n\t\t\t\t\t\t\t\tmargin: 0;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t#footerText \r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tpadding-left: 24px;\r\n\t\t\t\t\t\t\t\tfont-size: 10px;\r\n\t\t\t\t\t\t\t\tcolor: #666666;\r\n\t\t\t\t\t\t\t}\r\n\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t.error {\r\n\t\t\t\t\t\t\t\tfont-weight: bold;\r\n\t\t\t\t\t\t\t\tcolor: red;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.gridHeading\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t  background-color:#A3A29D;\r\n\t\t\t\t\t\t\t  padding:10px 0px 10px 5px;\r\n\t\t\t\t\t\t\t  color:#FFFFFF;\r\n\t\t\t\t\t\t\t  font-weight:bold;\r\n\t\t\t\t\t\t\t  font-family:Arial;\r\n\t\t\t\t\t\t\t  font-size:12px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.gridHeaderRow\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t  background-color:#F2F2F4;\r\n\t\t\t\t\t\t\t  color:#666666;\r\n\t\t\t\t\t\t\t  padding:4px;\r\n\t\t\t\t\t\t\t  font-family: Arial;\r\n\t\t\t\t\t\t\t  font-size: 11px;\r\n\t\t\t\t\t\t\t  font-weight:bold;\r\n\t\t\t\t\t\t\t  border-bottom:solid 1px #E4E4E4;\r\n\t\t\t\t\t\t\t  padding: 0px 10px 0px 10px;\r\n\t\t\t\t\t\t\t  height:25px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.gridRow\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t  background-color:#FFFFFF;\r\n\t\t\t\t\t\t\t  color:#666666;\r\n\t\t\t\t\t\t\t  border-bottom:solid 1px #E4E4E4;\r\n\t\t\t\t\t\t\t  padding:4px;\r\n\t\t\t\t\t\t\t  font-family: Arial;\r\n\t\t\t\t\t\t\t  padding: 0px 10px 0px 10px;\r\n\t\t\t\t\t\t\t  font-size: 11px;\r\n\t\t\t\t\t\t\t  height:25px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.step-row\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t  padding: 0;\r\n\t\t\t\t\t\t\t  height: 70px;\r\n\t\t\t\t\t\t\t}\r\n\r\n\t\t\t\t\t\t\t.step-icon\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t  padding-left:16px;\r\n\t\t\t\t\t\t\t  padding-right:16px;\r\n\t\t\t\t\t\t\t  text-align:center;\r\n\t\t\t\t\t\t\t}\r\n\t\t\t\t\t\t</style>\r\n</head>\r\n<body>\r\n<table id=\"mailWrapper\" cellpadding=\"0\" cellspacing=\"0\">\r\n<tbody>\r\n<tr>\r\n<td colspan=\"2\">\r\n<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n<tbody>\r\n<tr>\r\n<td style=\"background-color:#000000;height:96px;padding-left: 4px;padding-right: 4px;margin:0px;\" valign=\"center\">\r\n<img alt=\"header\" style=\"border:0;padding:0px; margin:0px;display:block;\" src=\"https://www.paynetexchange.com/sungard/payments/images/sungard/\\sungard.gif\r\n\t\t\t\t\t\t\t\t\t\t\t\"></td>\r\n<td style=\"background-color:#014F5B;padding-left:4px;\" width=\"100%\"><span style=\"font-family:Arial; font-weight:bold; font-size:17px; color:#FFFFFF;padding-left:4px;\">PayNetExchange</span></td>\r\n<td><img alt=\"header\" style=\"border:0;padding:0px; margin:0px;display:block;\" src=\"https://www.paynetexchange.com/sungard/payments/images/sungard/\\bg-header.gif\r\n\t\t\t\t\t\t\t\t\t\t\t\"></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<br>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td id=\"mailBody\"><br>\r\n<br>\r\n<h1>Card Blocked</h1>\r\n<p>A credit card payment has been made by <strong>The Feast</strong> to <strong>LARS ULRICH.\r\n</strong>has been blocked.<br>\r\n<br>\r\n</p>\r\n<p class=\"error\">This card may no longer be processed. </p>\r\n<table cellpadding=\"0\" cellspacing=\"0\">\r\n<tbody>\r\n<tr>\r\n<td style=\"text-align: right; width: 150px; padding-right: 8px;\">Amount: </td>\r\n<td>$1000</td>\r\n</tr>\r\n<tr>\r\n<td style=\"text-align: right; width: 150px; padding-right: 8px;\">Payment Number: </td>\r\n<td>6001109121</td>\r\n</tr>\r\n<tr>\r\n<td style=\"text-align: right; width: 150px; padding-right: 8px;\">Account Number: <br>\r\n<br>\r\n</td>\r\n<td>IDDQD<br>\r\n<br>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td style=\"text-align: right; width: 150px; padding-right: 8px;\" valign=\"top\">Payer:\r\n</td>\r\n<td>The Feast<br>\r\n\r\nMantoza, FL 32206<br>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n<td valign=\"top\" align=\"right\"></td>\r\n</tr>\r\n<tr>\r\n<td colspan=\"2\" style=\"padding-left: 32px; padding-right: 15px;\">\r\n<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n<tbody>\r\n<tr>\r\n<td width=\"16%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"left\">Invoice Number</td>\r\n<td width=\"13%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"left\">Invoice Date</td>\r\n<td width=\"30%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"left\">Description</td>\r\n<td width=\"15%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"right\">Gross Amount</td>\r\n<td width=\"13%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"right\">Discount</td>\r\n<td width=\"13%\" class=\"gridHeaderRow\" valign=\"middle\" align=\"right\">Net Amount</td>\r\n</tr>\r\n<tr>\r\n<td valign=\"middle\" width=\"16%\" class=\"gridRow\" align=\"left\">9999000111</td>\r\n<td valign=\"middle\" width=\"13%\" class=\"gridRow\" align=\"left\">08-29-2009</td>\r\n<td valign=\"middle\" width=\"30%\" class=\"gridRow\" align=\"left\">Group Management</td>\r\n<td valign=\"middle\" width=\"15%\" class=\"gridRow\" align=\"right\">$1,000.00</td>\r\n<td valign=\"middle\" width=\"13%\" class=\"gridRow\" align=\"right\">$0.00</td>\r\n<td valign=\"middle\" width=\"13%\" class=\"gridRow\" align=\"right\">$1,000.00</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n<tr>\r\n<td colspan=\"2\" style=\"text-align: center;\"><br>\r\n<br>\r\n<hr style=\"height: 1px; width: 670px;\">\r\n</td>\r\n</tr>\r\n<tr>\r\n<td class=\"mailBody\" colspan=\"2\">\r\n<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" class=\"footerTable\">\r\n<tbody>\r\n<tr>\r\n<td id=\"footerText\">© 2014 SunGard\r\n<p>Trademark information: SunGard and the SunGard logo are trademarks or registered trademarks of SunGard Data Systems Inc. or its subsidiaries in the U.S. and other countries. All other trade names are trademarks or registered trademarks of their respective\r\n holders. </p>\r\n</td>\r\n<td id=\"footerLogo\"><img alt=\"SunGard PayNetExchange\" src=\"https://www.paynetexchange.com/sungard/payments/images/sungard/\\thinkbeforeprint.jpg\r\n\t\t\t\t\t\t\t\t\t\t\t\t\"></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n</body>\r\n</html>\r\n";
		}
	}
}
