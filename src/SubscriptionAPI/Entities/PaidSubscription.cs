﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SubscriptionAPI.Entities
{
    public class PaidSubscription
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public Tariff Tariff { get; set; }
        [Key]
        public int SubscribedToId { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsAutorenewal {get;set; }
    }
}
