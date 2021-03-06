﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using htcustomer.entity.Enums;

namespace htcustomer.service.Helper
{
    public class JsonMessage
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public int ConfirmId { get; set; } // use when success adding 
    }
}
