using htcustomer.entity;
using htcustomer.service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htcustomer.service.Interface
{
    public interface IAuthService
    {
        string RegisterAccount(RegistrationView register);
        bool ActivateAccount(string id);
    }
}
