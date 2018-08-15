using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Care.Models
{
    public class DatasetModel
    {
        
        public List<Disease> DiseaseList { get; set; }
        public ISet<string> AllSymptomSet { get; set; }
        public List<SymptomWithOccurance> SymptomWithOccuranceList { get; set; }


        public DatasetModel()
        {
            DiseaseList = new List<Disease>();
            string path = AppDomain.CurrentDomain.BaseDirectory + "Data\\dataset.csv";
            using (var reader = new StreamReader(path))
            {

                while (!reader.EndOfStream)
                {
                    Disease d = new Disease();
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    d.Name = values[0];

                    for (int i = 1; i < values.Length; i++)
                    {
                        d.Symptoms.Add(values[i]);
                    }
                    DiseaseList.Add(d);

                }

                AllSymptomSet = new HashSet<string>();
                SymptomWithOccuranceList = new List<SymptomWithOccurance>();

                foreach (Disease disease in DiseaseList)
                {
                    foreach (var symptom in disease.Symptoms)
                    {
                        AllSymptomSet.Add(symptom);

                        //Add to Symptom and Occurance list
                        bool Added = false;
                        foreach (SymptomWithOccurance swo in SymptomWithOccuranceList)
                        {
                            if (swo.Name.Equals(symptom))
                            {
                                swo.Count++;
                                Added = true;
                                break;
                            }
                        }
                        if (!Added)
                        {
                            SymptomWithOccurance swo = new SymptomWithOccurance
                            {
                                Name = symptom,
                                Count = 1
                            };
                            SymptomWithOccuranceList.Add(swo);
                        }
                    }
                }

                SymptomWithOccuranceList.Sort((x, y) => y.Count.CompareTo(x.Count));
            }
        }

        public IEnumerable<string> GetAllSymptoms()
        {
            return AllSymptomSet;
        }

        public IEnumerable<string> GetAllDiseaseNames()
        {
            List<string> names = new List<string>();

            foreach (var disease in DiseaseList)
            {
                names.Add(disease.Name);
            }

            return names;
        }

        public Disease GetDiseaseInfo(string name)
        {
            foreach (var disease in DiseaseList)
            {
                if (disease.Name.Equals(name)) return disease;
            }

            return null;
        }



        public IEnumerable<string> GetCommonSymptoms(int count)
        {
            return GetCommonSymptoms().Take(count);
        }
        public IEnumerable<string> GetCommonSymptoms()
        {
            List<string> syms = new List<string>();
            for (int i = 0; i < SymptomWithOccuranceList.Count; i++)
            {
                syms.Add(SymptomWithOccuranceList[i].Name);
            }

            return syms;
        }

        public IEnumerable<DiseaseMatchedSymptoms> DiseaseMatchedSymptoms(List<string> symptoms)
        {
            List<DiseaseMatchedSymptoms> diseaseMatchedSymptoms = new List<DiseaseMatchedSymptoms>();

            foreach (Disease disease in this.DiseaseList)
            {
                int count = disease.Symptoms.Intersect(symptoms).Count();
                diseaseMatchedSymptoms.Add(new DiseaseMatchedSymptoms()
                {
                    Disease = disease,
                    MatchedSymptoms = count
                });
            }

            diseaseMatchedSymptoms.Sort((x, y) => y.MatchedSymptoms.CompareTo(x.MatchedSymptoms));

            return diseaseMatchedSymptoms;
        }

        public List<string> GetNextSymptoms(List<string> syms, List<string> notSyms)
        {
            IEnumerable<DiseaseMatchedSymptoms> diseaseMatchedSymptomsList = DiseaseMatchedSymptoms(syms);
            List<string> newSyms = new List<string>();
            foreach (var diseaseMatchedSymptoms in diseaseMatchedSymptomsList)
            {
                List<string> dSymps = diseaseMatchedSymptoms.Disease.Symptoms;
                foreach (var dSymp in dSymps)
                {
                    if (!syms.Contains(dSymp))
                    {
                        newSyms.Add(dSymp);
                    }
                }
            }

            foreach (var newSym in newSyms.ToArray())
            {
                if (notSyms.Contains(newSym))
                {
                    newSyms.Remove(newSym);
                }
            }

            return newSyms;

        }
    }

    public class SymptomWithOccurance
    {
        public int Count { get; set; }
        public string Name { get; set; }
    }
}