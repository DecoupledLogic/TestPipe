using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestPipe.Email;

namespace TestPipe.Test.Email
{
    [TestClass]
    public class ExchangeServiceTest2
    {
        [TestMethod]
        public void Count_Unread2()
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
        public void Find_Message2()
        {

            ExchangeService svc = new ExchangeService(ExchangeVersion.Exchange2010_SP2);
            svc.Url = new Uri("https://voo-casarray01.internal.sungard.corp/EWS/Exchange.asmx");
            MailBox sut = new MailBox(svc);

            ItemView viewEmails = new ItemView(int.MaxValue) { PropertySet = new PropertySet(BasePropertySet.IdOnly) };
            var contactEmailAddress2 = "agp.jax.vendorenrollment@sungard.com";
            var contactEmailAddress = "agp.jax.CustomerSupport@sungard.com";
            var queryString = string.Format("(From:={0})", contactEmailAddress);
            var queryString2 = string.Format("(From:={0})", contactEmailAddress2);
            FindItemsResults<Item> findResults = svc.FindItems(WellKnownFolderName.Inbox, queryString2, viewEmails);
            svc.LoadPropertiesForItems(findResults.Items, PropertySet.FirstClassProperties);

            string result = string.Empty;
            string infileResult = string.Empty;
            //string patternInvoice = "<td valign=\"middle\" width=\"16%\" class=\"gridRow\" align=\"left\">\\d{10}</td>";
            //string patternInvoiceReplace = "<td valign=\"middle\" width=\"16%\" class=\"gridRow\" align=\"left\"></td>";
            //string patternGroup = "<td valign=\"middle\" width=\"30%\" class=\"gridRow\" align=\"left\">[\\sa-zA-Z0-9]{0,}</td>";
            //string patternGropuReplace = "<td valign=\"middle\" width=\"30%\" class=\"gridRow\" align=\"left\"></td>";
            if (findResults.Items.Count > 0) // Prevent the exception -- "You must load or assign this property before you can read its value."
            {
                //foreach (Item item in findResults)
                //{
                string path = Path.GetDirectoryName(@"C:\Users\Darshit.dave\Downloads\EmailChecker\CustomerFromMail.txt");
                result = findResults.Items[0].Body;
                using (StreamWriter vendorOutFile = new StreamWriter(path + @"\VendorMail.txt"))
                {
                    vendorOutFile.Write(result);
                }
                //}
                //string path = Path.GetDirectoryName(@"C:\Users\Darshit.dave\Downloads\EmailChecker\CustomerFromMail.txt");
                //using (StreamWriter outfile = new StreamWriter(path + @"\CustomerFromMail.txt"))
                //{
                //    outfile.Write(result);
                //}

                //using (StreamReader infile = new StreamReader(path + @"\CustomerEmail.txt"))
                //{
                //    infileResult = infile.ReadToEnd();
                //}

                //bool mailCompare = StringComparer.OrdinalIgnoreCase.Equals(result, infileResult);
                // \p{Sc}{1}\d
                //infileResult = Regex.Replace(infileResult, @"Address1<br>\r\n", "");
                //infileResult = Regex.Replace(infileResult, @"\bAddress1<br>\b", "dfg");



                //infileResult = Regex.Replace(infileResult, @"The Feast", "payer");
                //infileResult = Regex.Replace(infileResult, @"LARS ULRICH", "company_name");
                //infileResult = Regex.Replace(infileResult, @"3\.{1}", "block_reason");
                //infileResult = Regex.Replace(infileResult, @"\p{Sc}\d{1,}(\.{0,1}|\,{0,1})\d{1,}\.{0,1}\d{1,}", "");
                //infileResult = Regex.Replace(infileResult, @"6001109121", "payment_number");
                //infileResult = Regex.Replace(infileResult, @"IDDQD", "account_number");
                //infileResult = Regex.Replace(infileResult, @"1990 Neptune plaza", "address1");
                //infileResult = Regex.Replace(infileResult, @"1100", "address2");
                //infileResult = Regex.Replace(infileResult, @"Left", "address3");
                //infileResult = Regex.Replace(infileResult, @"Mantoza", "city");
                //infileResult = Regex.Replace(infileResult, @"FL", "state");
                //infileResult = Regex.Replace(infileResult, @"32206", "zip");
                //infileResult = Regex.Replace(infileResult, @"\d{1,2}-\d{1,2}-\d{4}", "");
                //infileResult = Regex.Replace(infileResult, patternInvoice, patternInvoiceReplace);
                //infileResult = Regex.Replace(infileResult, patternGroup, patternGropuReplace);



                //infileResult = Regex.Replace(infileResult, , "");
                //infileResult = Regex.Replace(infileResult, @"\p{Sc}{1}1,0{3}.0{2}", "");
                //infileResult = Regex.Replace(infileResult, @"\p{Sc}{1}0.0{2}", "");

                //using (StreamWriter outfile = new StreamWriter(path + @"\CustomerEmail.txt"))
                //{
                //    outfile.Write(infileResult);
                //}

                //using (StreamWriter outfile = new StreamWriter(path + @"\DefaultCustomer.txt"))
                //{
                //    outfile.Write(infileResult);
                //}

                //using (StreamWriter outfile = new StreamWriter(path + @"\DefaultCustomerHtml.html"))
                //{
                //    outfile.Write(infileResult);
                //}
                //Assert.IsTrue(result.Length > 0);
            }
        }
    }
}
