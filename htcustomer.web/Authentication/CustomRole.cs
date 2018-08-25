using htcustomer.entity;
using htcustomer.service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Unity.Attributes;

namespace htcustomer.web.Authentication
{
    public class CustomRole : RoleProvider
    {
        private IAuthService authService;

        [InjectionConstructor]
        public CustomRole(IAuthService _authService)
        {
            authService = _authService;
        }
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
            var roles = authService.GetRolesForUser(username);
            if (roles.Count > 0)
            {
                userRoles = new[] { roles.Select(r => r.RoleName).ToString() };
            }
            return userRoles;          
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
        public CustomRole()
        : this(DependencyResolver.Current.GetService<IAuthService>())
        { }

    }
}