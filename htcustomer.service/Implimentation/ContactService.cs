using htcustomer.repository;
using htcustomer.service.Interfaces;
using htcustomer.entity;
using System.Collections.Generic;
using System.Linq;
using htcustomer.service.ViewModel;
using htcustomer.service.ViewModel.Contact;
using System;

namespace htcustomer.service.Implements
{
    public class ContactService : IContactService
    {
        private readonly IRepository<Customer> customerRepository;
        private readonly IUnitOfWork unitOfWork;
        public ContactService(IUnitOfWork _unitOfWork, IRepository<Customer> _customerRepository)
        {
            unitOfWork = _unitOfWork;
            customerRepository = _customerRepository;
        }
        public IEnumerable<CustomerViewModel> GetAllCustomer()
        {
            var customer = customerRepository.Gets().Select(c => new CustomerViewModel
            {
                CustomerID = c.CustomerID,
                Name = c.Name,
                Phone = c.Phone,
                Address = c.Address,
                Description = c.Description
            });
            return customer;
        }
        public AddressBookViewModel GetAddressBook(string searchValue)
        {
            var customerList = customerRepository.Gets()
                .Where(e => (searchValue == null) || ((searchValue != null) && ((e.Name + " " + e.Description).ToUpper().Contains(searchValue.ToUpper()))))
                .Where(e => e.Disable != true)
                .Select(c => new CustomerViewModel
                {
                    CustomerID = c.CustomerID,
                    Name = c.Name,
                    Phone = c.Phone,
                    Address = c.Address,
                    Description = c.Description,
                    Disable = c.Disable
                })
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Description)
                .Take(20);

            var addressBook = new AddressBookViewModel { customerList = customerList };
            return addressBook;
        }
        // Existed Name + Description  - return true if existed
        public bool Existed(CustomerViewModel customer)
        {
            var result = customerRepository
                .Gets()
                .Where(c => c.Name.ToUpper().Equals(customer.Name.ToUpper()) && (customer.Description == null || c.Description.ToUpper().Equals(customer.Description.ToUpper())));    
            if (!result.Any())
                return false;
            return true;
        }

        public int AddCustomer(CustomerViewModel customer)
        {
            if (customer == null) throw new ArgumentNullException("Null Argument");
            if (Existed(customer) == true) return -1;

            customer.Disable = false;
            var newCustomer = new Customer
            {
                Name = customer.Name,
                Description = customer.Description,
                Phone = customer.Phone,
                Address = customer.Address,
                Disable = false
            };
            customerRepository.Insert(newCustomer);
            unitOfWork.SaveChanges();
            return newCustomer.CustomerID;
        }

        public void DisableCustomer(int customerID)
        {
            if (customerID == 0) throw new ArgumentNullException("Null Argument");                        
            try
            {
                var customer = customerRepository.GetByID(customerID);
                customer.Disable = true;
                customerRepository.Edit(customer);
                unitOfWork.SaveChanges();
            } 
            catch(Exception ex)
            {
                throw new Exception("Error during remove customer");
            }
            
        }

        public bool UpdateCustomer(CustomerViewModel customer)
        {
            var entity = customerRepository.GetByID(customer.CustomerID);
            if (entity == null) throw new ArgumentNullException("Null Argument");
            if (Existed(customer) == true) return false;            
            try
            {
                entity.Name = customer.Name;
                entity.Description = customer.Description;
                entity.Phone = customer.Phone;
                entity.Address = customer.Address;
                entity.Disable = customer.Disable;
                customerRepository.Edit(entity);
                unitOfWork.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception("Error during updating");
            }
            return true; 
        }

        public IEnumerable<CustomerSearchViewModel> SearchCustomer(string searchValue)
        {
            var customerList = customerRepository.Gets().Where(x => !x.Disable.HasValue || !x.Disable.Value)
                                                        .Where(x => (x.Name.ToLower() + ' ' + (x.Description != null? x.Description.ToLower() : "") + ' ' + x.Phone).Contains(searchValue.ToLower()))
                                                        .Select(x => new CustomerSearchViewModel
                                                        {
                                                            CustomerId = x.CustomerID,
                                                            CustomerName = x.Name + (x.Description != null ? ' ' + x.Description : ""),
                                                            Phone = x.Phone
                                                        });
            return customerList;
        }
    }
}
