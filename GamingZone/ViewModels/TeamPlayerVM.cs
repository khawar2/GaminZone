using GamingZone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GamingZone.ViewModels
{
    public class TeamPlayerVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string TeamName { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
    public class AllPlayers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public int TeamId { get; set; }
        public string ImagePath { get; set; }

        public virtual Team Team { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}