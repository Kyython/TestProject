using System;

namespace TestProject.Models
{
    public class ResponseServiceInformation : Entity
    {
        public string NameService { get; set; }

        public bool IsAvailable { get; set; }

        public int? RequestTime { get; set; }
    }
}
