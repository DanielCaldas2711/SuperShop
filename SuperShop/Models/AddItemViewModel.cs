﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class AddItemViewModel
    {
        [Display(Name = "Product")]
        [Range(1,int.MaxValue, ErrorMessage = "You must select a product")]
        public int ProductId { get; set; }

        [Range(0.0001, double.MaxValue, ErrorMessage = "The quantity must be a positive number.")]
        public double Quantity { get; set; }

        public IEnumerable<SelectListItem> Products { get; set; } //Já recebe logo os items prontos para serem inseridos em HTML em selectlist ou MultiSelectList
    }
}
