using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Care.Models
{
    public class PatientState
    {
        public List<string> Symptoms { get; set; }
        public List<string> NotSymptoms { get; set; }
    }
}