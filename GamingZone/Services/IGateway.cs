using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GamingZone.ViewModels;

namespace GamingZone.Services
{
    public interface IGateway
    {
        PaymentResult ProcessPayment(CheckoutViewModel model);
    }
}