using System;
using System.Collections.Generic;
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
            SearchFilter filter = new SearchFilter.ContainsSubstring(ItemSchema.Subject, "PayNetExchange");
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
	}
}
