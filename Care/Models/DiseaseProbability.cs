using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Care.Models
{
    public class DiseaseProbability
    {
        public Disease Disease { get; set; }
        public float Probability { get; set; }
    }

    public class DiseaseMatchedSymptoms
    {
        public Disease Disease { get; set; }
        public int MatchedSymptoms { get; set; }
    }
}