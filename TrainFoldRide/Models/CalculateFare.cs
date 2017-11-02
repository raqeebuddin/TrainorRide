using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Tfl.Api.Presentation.Entities.Fares;
using System.Diagnostics;

namespace TrainFoldRide.Models
{
    public class CalculateFare
    {
        //requires StationAtcoCode e.g. 940GZZLSTB
        //returns cost e.g. 4.90 (no £ pound sign included)
        public string FareFunc(string depStation, string arrStation)
        {
            var fareCall = String.Format(
               $"https://api.tfl.gov.uk/StopPoint/{depStation}/FareTo/{arrStation}");
            //IEnumerable<FareDetails> list = new List<FareDetails>();
            
            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString(fareCall);
                var data = JsonConvert.DeserializeObject<List<FaresSection>>(json);
                List<FareDetails> rowList = data[0].Rows.ToList();
                List<Ticket> ticketsList = rowList[0].TicketsAvailable.ToList();
         
                Debug.WriteLine(ticketsList[0].Cost);
                return ticketsList[0].Cost;
               
            }
        }
    }
}