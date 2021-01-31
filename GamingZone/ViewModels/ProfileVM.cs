using GamingZone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamingZone.ViewModels
{
    public class ProfileVM
    {
        public Player Players { get; set; }
        public Rating Ratings { get; set; }
    }
}