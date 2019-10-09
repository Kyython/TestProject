using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Models
{
    public class ResponseInformationViewModel
    {
        public string NameService { get; set; }

        public bool? IsAvailable { get; set; }

        public int? RequestTime { get; set; }

        public int CountFailuresPerHour { get; set; }

        public int CountFailuresPerDay { get; set; }

        public double DiscrepancyPerHour { get; set; }

        public double DiscrepancyPerDay { get; set; }
    }
}
