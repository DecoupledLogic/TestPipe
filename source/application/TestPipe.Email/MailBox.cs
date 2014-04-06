namespace TestPipe.Email
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Exchange.WebServices.Data;

    public class MailBox
    {
        private ExchangeService service;

        public MailBox(ExchangeService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Empties the specified folder.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The number of items deleted from the folder.</returns>
        public int Empty(MailFilter filter)
        {
            FindItemsResults<Item> results = this.FindItems(filter);

            if (results == null)
            {
                return 0;
            }

            int count = results.TotalCount;

            foreach (var item in results)
            {
                item.Delete(DeleteMode.HardDelete);
                
							//DeleteMode.MoveToDeletedItems;
                //DeleteMode.SoftDelete
            }

            return count;
        }

        /// <summary>
        /// Finds the <see cref="MailItem"/>s specified by the filter.
        /// </summary>
        /// <param name="filter">MailFilter filter.</param>
        /// <returns>ICollection&lt;<see cref="MailItem"/>&gt;</returns>
        public ICollection<MailItem> Find(MailFilter filter)
        {
            FindItemsResults<Item> findResults = this.FindItems(filter);

            ServiceResponseCollection<GetItemResponse> items =
                this.service.BindToItems(
                    findResults.Select(item => item.Id),
                    new PropertySet(
                        BasePropertySet.FirstClassProperties,
                        EmailMessageSchema.From,
                        EmailMessageSchema.ToRecipients));

            return items.Select(item =>
            {
                string from = ((Microsoft.Exchange.WebServices.Data.EmailAddress)item.Item[EmailMessageSchema.From]).Address;
                string[] recipients = ((Microsoft.Exchange.WebServices.Data.EmailAddressCollection)item.Item[EmailMessageSchema.ToRecipients]).Select(recipient => recipient.Address).ToArray();
                string subject = item.Item.Subject;
                string body = item.Item.Body.ToString();
                return new MailItem(from, recipients, subject, body);
            }).ToArray();
        }

        private FindItemsResults<Item> FindItems(MailFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            if (string.IsNullOrWhiteSpace(filter.Subject) && string.IsNullOrWhiteSpace(filter.Body))
            {
                throw new ArgumentException("The MailFilter must have a valid Subject or Body defined.");
            }

            List<SearchFilter> searchFilterCollection = new List<SearchFilter>();
            searchFilterCollection.Add(new SearchFilter.ContainsSubstring(ItemSchema.Subject, filter.Subject));
            searchFilterCollection.Add(new SearchFilter.ContainsSubstring(ItemSchema.Body, filter.Body));

            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(filter.Operator, searchFilterCollection.ToArray());

            ItemView view = new ItemView(filter.ResultsLimit);

            // Limit the property set to the property ID for the base property set, and the subject and item class for the additional properties to retrieve.
            view.PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject, ItemSchema.ItemClass);

            // Setting the traversal to shallow will return all non-soft-deleted items in the specified folder.
            view.Traversal = ItemTraversal.Shallow;

            FindItemsResults<Item> findResults = this.service.FindItems(filter.ParentFolder, searchFilter, view);

            return findResults;
        }
    }
}