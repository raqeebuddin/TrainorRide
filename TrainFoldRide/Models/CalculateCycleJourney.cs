using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;

namespace TrainFoldRide.Models
{
    public class CalculateCycleJourney
    {
        public List<Leg> JourneyPlannerCycle(string depStation, string arrStation)
        {
            var JourneyCall = String.Format(
               $"https://api.tfl.gov.uk/journey/journeyresults/{depStation}/to/{arrStation}?&mode=cycle&bikeProficiency=Fast");

            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString(JourneyCall);
                ItineraryResult data = JsonConvert.DeserializeObject<TrainFoldRide.Models.ItineraryResult>(json);

                return data.Journeys[0].Legs;
            }
        }
    }
}