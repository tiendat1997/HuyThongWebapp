using htcustomer.entity.Enums;
using htcustomer.service.ViewModel.Contact;
using htcustomer.service.ViewModel.Device;
using htcustomer.service.ViewModel.Transaction;
using System.Collections.Generic;

namespace htcustomer.service.Interfaces
{
    public interface ITransactionService
    {        
        ContactDetailsViewModel GetContactDetails(int customerID);
        TransactionListHomeViewModel GetListTransactionHome();
        TransactionListViewModel GetListTransaction(TransactionStatus? status = null, int? month = null, int? year = null, int? categoryId = null);
        bool FixedTransaction(int transactionID, IEnumerable<PriceDetailViewModel> priceDetails);
        bool CannotFixTransaction(int transactionID, string reason = "");
        TransactionViewModel DeliverTransaction(int transactionID);
        bool Add(TransactionViewModel transaction);
        bool Add(TransactionCreateViewModel model);
        PriceTransactionViewModel GetTransactionToAddPrice(int transactionId);
        TransactionHomeViewModel GetTransactionToReload(int transactionId);
    }
}
