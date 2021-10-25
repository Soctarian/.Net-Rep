using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Controllers;
using DeserializeObjects;
using UserClasses;

namespace Project
{
    //Test DotaID: 268677900  Test SteamID: 76561198228943628 Test GameID: 6229091942 ...
    //Клас, що буде ліпити урли + ключ та базову частину посилання та комбінувати об'єкти в правильні урли???
    class Program
    {

        static void Main(string[] args)
        {
            InputMatchInfo output = new InputMatchInfo();

            /*    Console.Write("Input profile ID: ");
                var accountId = Convert.ToDecimal(Console.ReadLine());
                Console.Write("Input number of matches: ");
                var countMatches = Convert.ToInt32(Console.ReadLine());
                output.InputQuickMatchStatistic(countMatches, accountId);

                Console.Write("Input Match ID: ");
                var MatchID = Convert.ToDecimal(Console.ReadLine());
                output.InputFullkMatchStatistic(MatchID);

                Console.Write("Input hero name you wanna get info about: ");
                var Hero = Console.ReadLine();
                output.InputDetailMatchStatistic(MatchID, Hero);*/


            //test bd connections (witn sql studio)
            using (UserContext db = new UserContext())
            {
                User user1 = new User { SteamID = 76561198228943628, RealName = "Soctarian" };
                db.Users.Add(user1);
                db.SaveChanges();

                Console.WriteLine("Объекты успешно сохранены");

                var users = db.Users;
                Console.WriteLine("Список объектов:");
                foreach (User user in users)
                {
                    Console.WriteLine("{0} - {1}", user.SteamID, user.RealName);
                }
            }

            Console.ReadKey();
        }
    }
}
