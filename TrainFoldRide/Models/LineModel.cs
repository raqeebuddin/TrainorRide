using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainFoldRide.Models
{

    public class ValidityPeriod
    {
        public DateTime FromDate { get; set; }
        public bool IsNow { get; set; }
    }

    public class Disruption
    {
        public string Category { get; set; }
        public string CategoryDescription { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime Created { get; set; }
        public IEnumerable<string> AffectedRoutes { get; set; }
        public IEnumerable<string> AffectedStops { get; set; }
        public bool IsBlocking { get; set; }
        public string ClosureText { get; set; }
    }

    public class LineStatus
    {
        public string Id { get; set; }
        public int StatusSeverity { get; set; }
        public string StatusSeverityDescription { get; set; }
        public DateTime Created { get; set; }
        public IEnumerable<ValidityPeriod> ValidityPeriods { get; set; }
        public Disruption Disruption { get; set; }
        public string Reason { get; set; }
    }

    public class Line
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ModeName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public IEnumerable<string> DisruptedNaptanIds { get; set; }
        public IEnumerable<LineStatus> LineStatuses { get; set; }
        public IEnumerable<string> RouteSections { get; set; }
    }

    public class SearchMatch
    {
        public string StationId { get; set; }
        public string IcsId { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
    }

    public class SearchResponse
    {
        public string Query { get; set; }
        public int From { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Provider { get; set; }
        public int Total { get; set; }
        public IEnumerable<SearchMatch> Matches { get; set; }
        public double? MaxScore { get; set; }
    }

    public class SearchStations
    {

        [Required(ErrorMessage = "Please enter a Departure Station name.")]
        [RegularExpression("^[a-zA-Z ]*$",ErrorMessage = "Please enter a valid TFL Station (numbers or symbols are NOT permitted")]
        [Display(Name = "Enter Departure Station")]
        public string depStation { get; set; }
        
        [Required (ErrorMessage = "Please enter a Arrival Station name.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter a valid TFL Station (numbers or symbols are NOT permitted")]
        [Display(Name = "Enter Arrival Station")]
        public string arrStation { get; set; }
    }

    public class ItineraryResult
    {
        public List<Journey> Journeys { get; set; }
    }

    public class Journey
    {
        public DateTime StartDateTime { get; set; }
        public int Duration { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public List<Leg> Legs { get; set; }
        public List<Leg> CycleLegs { get; set; }

    }

    public class Leg
    {
        public Instruction Instruction { get; set; }
        public int Duration { get; set; }
        public int DurationHours { get; set; }
        public int DurationMinutes { get; set; }
        public float Distance { get; set; }
    }

    public class Instruction
    {
        public string Summary { get; set; }
        public string Detailed { get; set; }
        
    }
}
