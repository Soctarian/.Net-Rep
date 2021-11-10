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
using Controllers.Menu;
using DeserializeObjects;
using UserClasses;


namespace Project
{
    //Test Account ID32: 268677900 898455820  Test SteamID: 76561198228943628, 76561198836531527 Test GameID: 6229091942 ...
    class Program
    {
        //запити за розкладом WindowsService
        //список гравців зробити, замість дублювання 
        //асинхронно посилати запити await Task.WhenAll(list запросов)  
        //style cop для красивого кода

        static void Main(string[] args)
        {
           // Console.Write("Input profile ID: ");
           // var accountId = Convert.ToDecimal(Console.ReadLine());

            RunMenu menu = new RunMenu();
            menu.LoginScreen();

            Console.ReadKey(true);
        }
    }
}
