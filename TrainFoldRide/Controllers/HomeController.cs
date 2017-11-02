using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrainFoldRide.Models;
using Tfl.Api.Presentation.Entities.JourneyPlanner;
using System.Diagnostics;

namespace TrainFoldRide.Controllers
{
    public class HomeController : Controller
    {
        private TFREntities2 db = new TFREntities2();
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SearchStations userSearch)
        {
            if (ModelState.IsValid)
            {

                var apiDep = String.Format(
                    $"https://api.tfl.gov.uk/Stoppoint/Search/{userSearch.depStation}?modes=tube&useMultiModalCall=false");
                var apiArr = String.Format(
                    $"https://api.tfl.gov.uk/Stoppoint/Search/{userSearch.arrStation}?modes=tube&useMultiModalCall=false");

                var depList = new List<string>();
                var arrList = new List<string>();

                using (WebClient client = new WebClient())
                {
                    var json = client.DownloadString(apiDep);
                    var data = JsonConvert.DeserializeObject<TrainFoldRide.Models.SearchResponse>(json);

                    foreach (var item in data.Matches)
                    {
                        depList.Add(item.Name);
                    }
                }

                using (WebClient client = new WebClient())
                {
                    var json = client.DownloadString(apiArr);
                    var data = JsonConvert.DeserializeObject<TrainFoldRide.Models.SearchResponse>(json);

                    foreach (var item in data.Matches)
                    {
                        arrList.Add(item.Name);
                    }
                }

                ViewBag.depStations = new SelectList(depList);
                ViewBag.arrStations = new SelectList(arrList);
                if (depList.Count == 0 || arrList.Count == 0)
                {
                    ViewBag.depStations = "No Station match found";
                    return View(userSearch);
                }
                return View("SearchResult");
            }
            ModelState.AddModelError("", "Please enter a station name (numbers and symbols are NOT permitted)");
            return View(userSearch);
        }

        public ActionResult CalculateJourney(string depStations, string arrStations)
        {
            // opens the SearchForStation calls the method within which takes string of explicit station name e.g Liverpool Street and returns StationAtcoCode
            SearchForStation searchDep = new SearchForStation();
            SearchForStation searchArr = new SearchForStation();
            CalculateJourney journey = new CalculateJourney();
            CalculateFare startFareCalc = new CalculateFare();
            CalculateCycleJourney cycleJourney = new CalculateCycleJourney();

            Station departure = searchDep.searchFunc(depStations);
            Station arrive = searchArr.searchFunc(arrStations);

            TrainFoldRide.Models.Journey journeyPlanner = journey.JourneyPlannerTube(departure.icsId, arrive.icsId);
            List<TrainFoldRide.Models.Leg> cycleLeg = cycleJourney.JourneyPlannerCycle(departure.icsId, arrive.icsId);
            journeyPlanner.CycleLegs = cycleLeg;
            //StationAtcoCode provided to FareFunc method to calculate fare and return price e.g 4.90
            string price = startFareCalc.FareFunc(departure.naptanId, arrive.naptanId);
            journeyPlanner.CycleLegs[0].Distance = Convert.ToInt32(journeyPlanner.CycleLegs[0].Distance) / 1000;

            ViewBag.calories = journeyPlanner.CycleLegs[0].Distance * 22;
            ViewBag.price = ("£"+price);
            ViewBag.departure = departure.station1;
            ViewBag.arrival = arrive.station1;

            journeyPlanner.Duration += 5;
            var time = TimeSpan.FromMinutes(journeyPlanner.Duration);
            var hours = (int)time.TotalHours;
            var minutes = time.Minutes;
            journeyPlanner.Legs[0].DurationHours = hours;
            journeyPlanner.Legs[0].DurationMinutes = minutes;

            var cycleTime = TimeSpan.FromMinutes(cycleLeg[0].Duration);
            var cycleHours = (int)cycleTime.TotalHours;
            var cycleMinutes = cycleTime.Minutes;
            cycleLeg[0].DurationHours = cycleHours;
            cycleLeg[0].DurationMinutes = cycleMinutes;

            Station saveJourney = new Station();
            saveJourney.depStation = departure.station1;
            saveJourney.arrStation = arrive.station1;
            saveJourney.fare = price;
            saveJourney.CaloriesBurned = Convert.ToInt32(journeyPlanner.CycleLegs[0].Distance * 22);

            db.Stations.Add(saveJourney);
            db.SaveChanges();


            return View(journeyPlanner);
        }

        public ActionResult StoredJourneys()
        {
            
            var journeys = from journeyEntry in db.Stations
                         select journeyEntry;
            return View(journeys);
        }

        public ActionResult Delete(int id)
        {
            Station journey = db.Stations.Find(id);
            return View(journey);
        }

        //By addition to ActionName attribute, it will see this as go through Delete action
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Station journey = db.Stations.Find(id);
            db.Stations.Remove(journey);
            db.SaveChanges();

            return RedirectToAction("StoredJourneys");
        }
    }
}