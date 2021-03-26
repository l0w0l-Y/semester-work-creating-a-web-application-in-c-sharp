﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class WithdrawalModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int UserID { get; set; }
        public int Sum { get; set; }
    }
}