using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrainFoldRide.Models
{
    public class SaveJourney
    {
        public string depStation { get; set; }
        public string arrStation { get; set; }
        public string fare { get; set; }
        public int CaloriesBurned { get; set; }
    }
}