﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.Edm.Library;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SuperShop.Data.Entities
{
    public class Order : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Order date")]
        [DisplayFormat(DataFormatString ="{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime OrderDate { get; set; }


        [Display(Name = "Delivery date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? DeliveryDate { get; set; }

        [Required]
        public User User { get; set; } //Ligação de um para um

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public IEnumerable<OrderDetail> Items { get; set; } //Ligação de um para muitos

        public int Lines => Items == null ? 0 : Items.Count();

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity => Items == null ? 0 : Items.Sum(i => i.Quantity);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Value => Items == null ? 0 : Items.Sum(i => i.Value);

        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime? OrderDateLocal => this.OrderDate == null ? null : this.OrderDate.ToLocalTime();
    }
}
