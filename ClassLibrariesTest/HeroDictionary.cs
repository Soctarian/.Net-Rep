using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DeserializeObjects
{
    public class HeroDictionary
    {


        public class Hero
        {
            public string name { get; set; }
            public int id { get; set; }
            public string localized_name { get; set; }
        }

        public class Result
        {
            public List<Hero> heroes { get; set; }
            public int status { get; set; }
            public int count { get; set; }
        }

        public class Root
        {
            public Result result { get; set; }
        }

        public string GetHeroString()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
             $"https://api.steampowered.com/IEconDOTA2_570/GetHeroes/v0001/?key=A80EC4AFFB0862E8476DFD2967292B79&language=English");

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string sReadData;

            return sReadData = sr.ReadToEnd();
        }

        public void FillDictionary(string data)
        {
            var deserializedData = JsonConvert.DeserializeObject<Root>(GetHeroString());

            Dictionary<int, string> Heroes = new Dictionary<int, string>();

            foreach(var hero in deserializedData.result.heroes)
            {
                Heroes.Add(hero.id, hero.localized_name); 
            } 
        }
        


    
   

    }
}
