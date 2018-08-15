using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using Care.Models;
using Newtonsoft.Json;

namespace Care.Controllers
{
    public class SymptomsController : ApiController
    {
        public DatasetModel dModel;
        public SymptomsController()
        {
            dModel = new DatasetModel();
        }

        [HttpGet]
        public IEnumerable<string> GetSymptoms()
        {   
            return dModel.GetCommonSymptoms();
        }

        [Route("api/estimate/prob/")]
        [HttpGet]
        public IEnumerable<DiseaseProbability> EstimateDisease(string symptoms)
        {
            List<string> symps = JsonConvert.DeserializeObject<List<string>>(symptoms);

            List<DiseaseMatchedSymptoms> diseaseMatchedSymptoms = dModel.DiseaseMatchedSymptoms(symps) as List<DiseaseMatchedSymptoms>;

            List<DiseaseProbability> diseaseProbabilities = new List<DiseaseProbability>();


            int returnCount = dModel.DiseaseList.Count;
            int totalWeight = 0;
            for (int i = 0; i < returnCount; i++)
            {
                diseaseProbabilities.Add(new DiseaseProbability
                {
                    Disease = diseaseMatchedSymptoms[i].Disease
                });
                totalWeight += diseaseMatchedSymptoms[i].MatchedSymptoms;
            }

            for (int i = 0; i < returnCount; i++)
            {
                diseaseProbabilities[i].Probability = (float) diseaseMatchedSymptoms[i].MatchedSymptoms / totalWeight;

            }

            return diseaseProbabilities;
        }

        [Route("api/symptom/query/")]
        [HttpGet]
        public List<string> GetQuerySentence(string symptom)
        {
            List<string> symps = JsonConvert.DeserializeObject<List<string>>(symptom);
            List<string> querys = new List<string>();
            foreach (var symp in symps)
            {
                querys.Add("Do you have " + symp + " ?");
            }

            return querys;
        }


        [Route("api/symptom/match/")]
        [HttpGet]
        public IEnumerable<DiseaseMatchedSymptoms> MatchSymptom(string symptoms)
        {
            List<string> symps = JsonConvert.DeserializeObject<List<string>>(symptoms);

            return dModel.DiseaseMatchedSymptoms(symps);
        }


        [Route("api/symptom/next/")]
        [HttpGet]
        public IEnumerable<string> NextSymptoms(string state)
        {
            PatientState patientState = JsonConvert.DeserializeObject<PatientState>(state);


            List<string> syms = patientState.Symptoms;
            List<string> notSyms = patientState.NotSymptoms;

            List<string> newSyms = dModel.GetNextSymptoms(syms, notSyms);

            return newSyms.Take(4);

        }


        [Route("api/test")]
        [HttpGet]
        public PatientState Test(string state)
        {


            PatientState patientState = JsonConvert.DeserializeObject<PatientState>(state);
            return patientState;
        }

        [Route("api/apis")]
        [HttpGet]
        public List<string> Apis()
        {
            List<string> list = new List<string>();
            list.Add("api/symptoms");
            list.Add("api/symptom/next/{symptoms}");
            list.Add("api/estimate/prob/{symptoms}");
            list.Add("api/symptom/match/{symptoms}");


            return list;
        }
    }

    public class State
    {
        public string Symp { get; set; }
        public int Id { get; set; }
    }
}
