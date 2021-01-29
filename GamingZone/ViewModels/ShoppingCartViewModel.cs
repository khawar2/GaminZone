using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GamingZone.Models;

namespace GamingZone.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<CartItem> Items { get; set; }
        public decimal Total { get; set; }
    }
}