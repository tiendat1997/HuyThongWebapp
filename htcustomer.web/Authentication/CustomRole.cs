using htcustomer.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace htcustomer.web.Authentication
{
    public class CustomRole : RoleProvider
    {        
        public override bool IsUserInRole(string username, string roleName)
        {
            var userRoles = GetRolesForUser(username);
            return userRoles.Contains(roleName);
        }    
        public override string[] GetRolesForUser(string username)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userRoles = new string[] { };

            using (HuythongDB dbContext = new HuythongDB())
            {
                var selectedUser = dbContext.Users.Include("Roles")
                        .Where(us => string.Compare(us.Username, username, StringComparison.OrdinalIgnoreCase) == 0)
                        .Select(us => us)            
                        .FirstOrDefault();
                if (selectedUser != null)               
                    userRoles = new[] { selectedUser.Roles.Select(r => r.RoleName).ToString() };                

                return userRoles.ToArray();
            }
        }
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        #region Override abstract class not use
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }        
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}