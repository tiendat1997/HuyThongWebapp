using htcustomer.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htcustomer.repository
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private HuythongDB dbContext;
        public UnitOfWork(HuythongDB _dbContext)
        {
            dbContext = _dbContext;             
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dbContext != null)
                {
                    dbContext.Dispose();
                    dbContext = null;
                }
            }
        }
    }
}
