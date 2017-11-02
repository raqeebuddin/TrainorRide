using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace TrainFoldRide.Models
{
    public class SearchForStation
    {
        private TFREntities2 db = new TFREntities2();
        public Station searchFunc(string stationName)
        {
            Station station = new Station();
            var apiDep = String.Format(
               $"https://api.tfl.gov.uk/StopPoint/Search?query={stationName}%20Street&modes=tube&maxResults=25&faresOnly=true");

            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString(apiDep);
                var data = JsonConvert.DeserializeObject<TrainFoldRide.Models.SearchResponse>(json);

                List<SearchMatch> fareList = data.Matches.ToList();
                station.naptanId = fareList[0].Id;
                station.station1 = fareList[0].Name;
                station.icsId = fareList[0].IcsId;
            }
            //db.Stations.Add(station);
            //db.SaveChanges();
            //at this point station.depStation is the StatonAtcoCode needed for Fare
            return station;
        }

    }
}