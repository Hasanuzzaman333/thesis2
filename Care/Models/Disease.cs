using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Care.Models
{
    public class Disease
    {
        public Disease()
        {
            Symptoms = new List<string>();
        }
        public string Name { get; set; }
        public List<string> Symptoms { get; set; }
    }
}