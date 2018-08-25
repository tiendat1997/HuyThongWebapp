using htcustomer.entity;
using htcustomer.repository;
using htcustomer.service.Interface;
using htcustomer.service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htcustomer.service.Implimentation
{
    public class AuthService : IAuthService
    {
        private IUnitOfWork unitOfWork;
        private IRepository<User> userRepo; 
        public AuthService(IUnitOfWork _unitOfWork, IRepository<User> _userRepo)
        {
            unitOfWork = _unitOfWork;
            userRepo = _userRepo;
        }

        public bool ActivateAccount(string id)
        {
            var userAccount = userRepo.Gets().Where(u => u.ActivationCode.ToString().Equals(id)).FirstOrDefault();
            if (userAccount != null)
            {
                userAccount.IsActive = true;
                unitOfWork.SaveChanges();
                return true;
            }
            return false;
        }

        public string RegisterAccount(RegistrationView register)
        {
            var user = new User
            {
                Username = register.Username,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                Password = register.Password,
                ActivationCode = Guid.NewGuid(),
            };

            userRepo.Insert(user);
            unitOfWork.SaveChanges();

            return user.ActivationCode.ToString();
        }
    }
}
