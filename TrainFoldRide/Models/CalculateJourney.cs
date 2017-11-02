using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using Tfl.Api.Presentation.Entities.JourneyPlanner;
using Tfl.Api.Presentation.Entities;

namespace TrainFoldRide.Models
{
    public class CalculateJourney
    {
        public Journey JourneyPlannerTube(string depStation, string arrStation)
        {
            var JourneyCall = String.Format(
               $"https://api.tfl.gov.uk/journey/journeyresults/{depStation}/to/{arrStation}?&mode=tube");
            
            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString(JourneyCall);
                ItineraryResult data = JsonConvert.DeserializeObject<TrainFoldRide.Models.ItineraryResult>(json);

                Debug.WriteLine("The duration of this journey is: " + data.Journeys[0].Duration + " minutes");
                return data.Journeys[0];
            }
        }
    }
}