﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htcustomer.service.ViewModel.Transaction
{
    public class TransactionCreateViewModel
    {
        public int CustomerId { get; set; }
        public IEnumerable<TransactionItemCreateViewModel> Transactions { get; set; }
    }
}
