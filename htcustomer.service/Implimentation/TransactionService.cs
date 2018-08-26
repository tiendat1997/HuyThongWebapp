using htcustomer.entity;
using htcustomer.entity.Enums;
using htcustomer.repository;
using htcustomer.service.Interfaces;
using htcustomer.service.ViewModel;
using htcustomer.service.ViewModel.Category;
using htcustomer.service.ViewModel.Contact;
using htcustomer.service.ViewModel.Device;
using htcustomer.service.ViewModel.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;

namespace htcustomer.service.Implements
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepository<Transaction> transactionRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly IRepository<DetailPrice> priceDetailRepository;
        private readonly IUnitOfWork unitOfWork;

        public TransactionService(
            IRepository<Transaction> _transactionRepository,
            IRepository<Customer> _customerRepository,
            IRepository<Category> _categoryRepository,
            IRepository<DetailPrice> _priceDetailRepository,
            IUnitOfWork _unitOfWork
            )
        {
            transactionRepository = _transactionRepository;
            customerRepository = _customerRepository;
            categoryRepository = _categoryRepository;
            priceDetailRepository = _priceDetailRepository;
            unitOfWork = _unitOfWork;
        }

        public bool Add(TransactionViewModel transaction)
        {
            if (transaction != null)
            {
                transactionRepository.Insert(new Transaction()
                {
                    CustomerID = transaction.Customer.CustomerID,
                    Delivered = false,
                    Description = transaction.Description,
                    Error = transaction.Error,
                    RecievedDate = DateTime.Now,
                    StatusID = (int)entity.Enums.TransactionStatus.NotFix,
                    TypeID = transaction.Category.CategoryID
                });
                unitOfWork.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CannotFixTransaction(int transactionID, string reason = "")
        {
            try
            {
                var transaction = transactionRepository.GetByID(transactionID);
                if (transaction != null)
                {
                    transaction.StatusID = (int)entity.Enums.TransactionStatus.CannotFix;
                    transaction.Reason = reason;
                    transactionRepository.Edit(transaction);
                    unitOfWork.SaveChanges();
                    return true;
                }                
            }
            catch(Exception ex)
            {
                return false;
            }
            return false;
        }

        public TransactionViewModel DeliverTransaction(int transactionID)
        {
            var transaction = transactionRepository.GetByID(transactionID);
            if (transaction != null)
            {
                transaction.Delivered = true;
                transaction.DeliverDate = DateTime.Now;
                unitOfWork.SaveChanges();
                return new TransactionViewModel() {
                    TransactionID = transaction.TransactionID,
                    Status = (entity.Enums.TransactionStatus)transaction.StatusID,
                    Category = new CategoryViewModel()
                    {
                        CategoryID = transaction.Category.CategoryID,
                        Name = transaction.Category.Name
                    },
                    Error = transaction.Error,
                    RecievedDate = transaction.RecievedDate,
                    DeliveredDate = transaction.DeliverDate,
                    Price = transaction.Price,
                    ListPriceDetail = transaction.DetailPrices.Select(p => new PriceDetailViewModel()
                    {
                        TransactionID = p.TransactionID.HasValue ? p.TransactionID.Value : 0 ,
                        Description = p.Description,
                        Price = p.Price.HasValue ? p.Price.Value : 0
                    })
                };
            }
            return null;
        }

        public bool FixedTransaction(int transactionID, IEnumerable<PriceDetailViewModel> priceDetails)
        {
            try
            {
                var transaction = transactionRepository.GetByID(transactionID);
                if (transaction != null)
                {
                    if (priceDetails != null || priceDetails.Count() > 0)
                    {
                        foreach (var priceDetail in priceDetails)
                        {
                            priceDetailRepository.Insert(new DetailPrice()
                            {
                                Description = priceDetail.Description,
                                Price = priceDetail.Price,
                                TransactionID = transactionID
                            });
                            transaction.Price = priceDetails.Sum(x => x.Price);
                        }
                    }
                    transaction.StatusID = (int)entity.Enums.TransactionStatus.Fixed;
                    transactionRepository.Edit(transaction);
                    unitOfWork.SaveChanges();
                    return true;
                }                
            }
            catch (Exception ex)
            {
                throw new Exception("Error during check fixed transaction");
            }
            return false;
        }

        public ContactDetailsViewModel GetContactDetails(int customerID)
        {
            ContactDetailsViewModel model = new ContactDetailsViewModel
            {
                Customer = customerRepository.Gets()
                            .Where(c => c.CustomerID == customerID && c.Disable != true)
                            .Select(c => new CustomerViewModel
                            {
                                CustomerID = c.CustomerID,
                                Name = c.Name,
                                Address = c.Address,
                                Description = c.Description,
                                Disable = c.Disable,
                                Phone = c.Phone
                            }).FirstOrDefault(),
                Transactions = transactionRepository.Gets().Where(t => t.CustomerID == customerID)
                            .Select(t => new TransactionViewModel
                            {
                                TransactionID = t.TransactionID,
                                Status = (entity.Enums.TransactionStatus)t.StatusID,
                                RecievedDate = t.RecievedDate,
                                DeliveredDate = t.DeliverDate,
                                Category = categoryRepository.Gets().Where(c => c.CategoryID == t.TypeID).Select(c => new CategoryViewModel()
                                {
                                    CategoryID = c.CategoryID,
                                    Name = c.Name
                                }).SingleOrDefault(),
                                Description = t.Description,
                                Error = t.Error,
                                Delivered = t.Delivered,
                                Price = t.Price,
                                Reason = t.Reason,
                                ListPriceDetail = t.DetailPrices.Select(p => new PriceDetailViewModel()
                                {
                                    TransactionID = p.TransactionID.HasValue ? p.TransactionID.Value : 0,
                                    Description = p.Description,
                                    Price = p.Price.HasValue ? p.Price.Value : 0
                                })
                            }),
            };
            return model;
        }
        

        public TransactionListViewModel GetListTransaction(entity.Enums.TransactionStatus? status = null, int? month = null, int? year = null, int? categoryId = null)
        {
            var transactions = transactionRepository.Gets();
            if (status != null)
            {
                if (status != entity.Enums.TransactionStatus.Delivered)
                {
                    transactions = transactions.Where(t => t.StatusID == (int)status && t.Delivered == false);
                }
                else
                {
                    transactions = transactions.Where(t => t.Delivered == true);
                }
            }
            if (month != null && year != null)
            {
                transactions = transactions.Where(t => t.RecievedDate.Value.Month == month && t.RecievedDate.Value.Year == year);
            }
            else
            {
                transactions = transactions.Where(t => t.RecievedDate.Value.Month == DateTime.Now.Month && t.RecievedDate.Value.Year == DateTime.Now.Year);
            }
            if (categoryId != null)
            {
                transactions = transactions.Where(t => t.TypeID == categoryId);
            }

            var result = new TransactionListViewModel
            {
                Status = status,
                Transactions = transactions.Select(t => new TransactionViewModel()
                {
                    TransactionID = t.TransactionID,
                    DeliveredDate = t.DeliverDate,
                    Description = t.Description,
                    Error = t.Error,
                    Price = t.Price,
                    Reason = t.Reason,
                    Delivered = t.Delivered,
                    RecievedDate = t.RecievedDate,
                    Status = (entity.Enums.TransactionStatus)t.StatusID,
                    Category = new CategoryViewModel
                    {
                        CategoryID = t.Category.CategoryID,
                        Name = t.Category.Name
                    },
                    Customer = new CustomerViewModel
                    {
                        CustomerID = t.Customer.CustomerID,
                        Name = t.Customer.Name,
                        Phone = t.Customer.Phone
                    },
                    ListPriceDetail = t.DetailPrices.Select(p => new PriceDetailViewModel()
                    {
                        TransactionID = p.TransactionID.Value,
                        Description = p.Description,
                        Price = p.Price.Value
                    })
                })
            };

            return result;
        }
        

        public TransactionListHomeViewModel GetListTransactionHome()
        {
            var transactions = transactionRepository.Gets()
                                                    //.Where(t => t.RecievedDate.Value.Month == DateTime.Now.Month && t.RecievedDate.Value.Year == DateTime.Now.Year
                                                    //              && !t.Delivered.Value)
                                                    .Where(t => t.Delivered.HasValue ? t.Delivered.Value == false : true)
                                                    .Select(t => new TransactionHomeViewModel()
                                                    {
                                                        TransactionID = t.TransactionID,
                                                        Category = new CategoryViewModel()
                                                        {
                                                            CategoryID = t.Category.CategoryID,
                                                            Name = t.Category.Name
                                                        },
                                                        Customer = new CustomerViewModel()
                                                        {
                                                            CustomerID = t.Customer.CustomerID,
                                                            Name = t.Customer.Name,
                                                            Phone = t.Customer.Phone
                                                        },
                                                        DeviceDescription = t.Description != null ? t.Description : "No description for this device",
                                                        Price = t.Price != null ? t.Price.Value : 0,
                                                        Error = t.Error != null ? t.Error : "",
                                                        Status = (entity.Enums.TransactionStatus)t.StatusID.Value,
                                                        CannotFixNote = t.Reason != null ? t.Reason : "No reason for this transaction",
                                                        ListPriceDetail = t.DetailPrices.Select(p => new PriceDetailViewModel()
                                                        {
                                                            TransactionID = p.TransactionID.Value,
                                                            Description = p.Description,
                                                            Price = p.Price.Value
                                                        })
                                                    });

            return new TransactionListHomeViewModel()
            {
                CannotFixTransactions = transactions.Where(t => t.Status == entity.Enums.TransactionStatus.CannotFix).ToList(),
                FixedTransactions = transactions.Where(t => t.Status == entity.Enums.TransactionStatus.Fixed).ToList(),
                NotFixTransactions = transactions.Where(t => t.Status == entity.Enums.TransactionStatus.NotFix).ToList()
            };
        }
        public TransactionHomeViewModel GetTransactionToReload(int transactionId)
        {
            var transaction = transactionRepository
                                .Gets()
                                .Where(tran => tran.TransactionID == transactionId)
                                .Select(t => new TransactionHomeViewModel
                                {
                                    TransactionID = t.TransactionID,
                                    Category = new CategoryViewModel()
                                    {
                                        CategoryID = t.Category.CategoryID,
                                        Name = t.Category.Name
                                    },
                                    Customer = new CustomerViewModel()
                                    {
                                        CustomerID = t.Customer.CustomerID,
                                        Name = t.Customer.Name,
                                        Phone = t.Customer.Phone
                                    },
                                    Error = t.Error,
                                    Price = t.Price ?? 0,
                                    Status = (entity.Enums.TransactionStatus)t.StatusID,
                                    DeviceDescription = t.Description,
                                    CannotFixNote = t.Reason,
                                    ListPriceDetail = t.DetailPrices.Select(p => new PriceDetailViewModel()
                                    {
                                        TransactionID = p.TransactionID.Value,
                                        Description = p.Description,
                                        Price = p.Price.Value
                                    })
                                }).FirstOrDefault();                                                    
            return transaction;                                
        }

        public PriceTransactionViewModel GetTransactionToAddPrice(int transactionId)
        {
            var result = new PriceTransactionViewModel
            {
                TransactionID = transactionId,
                PriceList = priceDetailRepository.Gets()
                    .Where(item => item.TransactionID == transactionId)
                    .Select(item => new PriceDetailViewModel
                    {
                        Price = item.Price ?? 0,
                        Description = item.Description
                    })
            };
            return result;
        }
    }
}
