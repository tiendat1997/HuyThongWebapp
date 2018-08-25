using htcustomer.entity;
using htcustomer.repository;
using htcustomer.service.Implimentation;
using htcustomer.service.Interface;
using System;
using System.Data.Entity;
using Unity;
using Unity.Lifetime;

namespace htcustomer.web
{
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion
        
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<HuythongDB>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

            // Services
            container.RegisterType<IAuthService, AuthService>();            
        }
    }
}