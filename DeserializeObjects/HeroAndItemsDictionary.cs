using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DeserializeObjects
{
    public class HeroAndItemsDictionary
    {
        public Dictionary<int, string> HeroDictionary = new Dictionary<int, string>();
        public Dictionary<int, string> ItemDictionary = new Dictionary<int, string>();

        public class Item
        {
            public int id { get; set; }
            public string name { get; set; }
            public int cost { get; set; }
            public int secret_shop { get; set; }
            public int side_shop { get; set; }
            public int recipe { get; set; }
            public string localized_name { get; set; }
        }

        public class Hero
        {
            public string name { get; set; }
            public int id { get; set; }
            public string localized_name { get; set; }
        }

        public class Result
        {
            public List<Item> items { get; set; }
            public List<Hero> heroes { get; set; }
            public int status { get; set; }
            public int count { get; set; }
        }

        public class Root
        {
            public Result result { get; set; }
        }






        static public T GetHeroString<T>(Uri url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                    return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }

        }

        static Root GetHeroData()
        {
            Uri url = new Uri($"https://api.steampowered.com/IEconDOTA2_570/GetHeroes/v0001/?key=A80EC4AFFB0862E8476DFD2967292B79&language=English");
            return GetHeroString<Root>(url);
        }
        static Root GetItemData()
        {
            Uri url = new Uri($"https://api.steampowered.com/IEconDOTA2_570/GetGameItems/V001/?key=A80EC4AFFB0862E8476DFD2967292B79&language=English");
            return GetHeroString<Root>(url);
        }

        public void FillHeroDictionary()
        {
            var deserializedData = GetHeroData();
            Dictionary<int, string> Heroes = new Dictionary<int, string>();
            foreach (var hero in deserializedData.result.heroes) this.HeroDictionary.Add(hero.id, hero.localized_name);
        }

        public void FillItemDictionary()
        {
            var deserializedData = GetItemData();
            Dictionary<int, string> Items = new Dictionary<int, string>();
            foreach (var item in deserializedData.result.items) ItemDictionary.Add(item.id, item.localized_name);
        }

    }
}
