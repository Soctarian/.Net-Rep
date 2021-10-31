using System;
using System.Collections.Generic;
using System.Text;
using UserClasses;
using static System.Console;

namespace Controllers.Menu
{
    public class RunMenu
    {

        private decimal profileID32;

        public RunMenu(decimal profileID32)
        {
            this.profileID32 = profileID32;
        }

        public void StartMenu()
        {
            RunMainMenu();
        }

        public void RunMainMenu()
        {
            string promt = "Welcome to my .Net project!";
            string[] options = { "Input match statistic", "Add user to db", "Comparasive", "Exit" };
            KeyboardMenu mainMenu = new KeyboardMenu(promt, options);
            int SelectedIndex = mainMenu.Run();
            switch (SelectedIndex)
            {
                case 0:
                    RunInputMatchStatistic();
                    break;
                case 1:
                    RunAddUserToDB();
                    break;
                case 2:
                    RunComparasive();
                    break;
                case 3:
                    Exit();
                    break;
                default:
                    break;
            }

        }

        public void RunInputMatchStatistic()
        {
            InputMatchInfo inputMatchInfo = new InputMatchInfo();
            string promt = "Choose what kind of statistic you wanna get: ";
            string[] options = { "Quick match statistic", "Full match statistic", "Back to main menu", "Exit" };
            KeyboardMenu matchStatisticMenu = new KeyboardMenu(promt, options);
            int selectedIndex = matchStatisticMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    Write("\nInput number of matches: ");
                    var countMathces = Convert.ToInt32(ReadLine());
                    inputMatchInfo.InputQuickMatchStatistic(countMathces, profileID32);

                    WriteLine("If you want to return to the match statistic menu, press any key");
                    ReadKey(true);
                    RunInputMatchStatistic();
                    break;
                case 1:
                    Write("Input match ID: ");
                    var matchID = Convert.ToDecimal(ReadLine());
                    inputMatchInfo.InputFullkMatchStatistic(matchID);

                    WriteLine("Press y, if you want to get statistic for exact hero");
                    ConsoleKeyInfo keyInfo = ReadKey(true);
                    ConsoleKey keyPressed = keyInfo.Key;
                    if (keyPressed == ConsoleKey.Y)
                    {
                        RunStatisticForHeroInMatch(matchID);
                    }
                    else
                    {
                        WriteLine("If you want to return to the match statistic menu, press any key");
                        ReadKey(true);
                        RunInputMatchStatistic();
                    }

                    break;
                case 2:
                    RunMainMenu();
                    break;
                case 3:
                    Exit();
                    break;
                default:
                    break;
            }

        }
        public void RunStatisticForHeroInMatch(decimal matchID)
        {
            InputMatchInfo inputMatchInfo = new InputMatchInfo();

            Write("Input hero name: ");
            var hero = ReadLine();
            inputMatchInfo.InputDetailMatchStatistic(hero, matchID);

            WriteLine("If you want to back to the statistic menu, press any key");
            ReadKey(true);
            RunInputMatchStatistic();

        }
        public void RunAddUserToDB()
        {
            AddUser.AttachUser(Convert.ToInt64(Deciphers.ConvertToSteamID64(profileID32)));
        }
        public void RunComparasive()
        {
            string promt = "Choose what kind of comparasive you wanna do";
            string[] options = { "Time played comparasive", "Winrate comparasive", "Back to main menu", "Exit" };
            KeyboardMenu сomparasiveMenu = new KeyboardMenu(promt, options);
            int selectedIndex = сomparasiveMenu.Run();
            switch (selectedIndex)
            {
                case 0:
                    Write("Enter the id of the player with whom you want to compare the time spent in the game: ");
                    var secondID = Convert.ToDecimal(ReadLine());
                    Write("Enter the number of days which you want to compare time for: ");
                    var time = Convert.ToInt32(ReadLine());
                    PlayerComparison comaprison = new PlayerComparison(profileID32, secondID, time);
                    InputComparisonInfo.InputTimeComparison(comaprison.TimeComparison(), time);

                    WriteLine("\nIf you want to back to the comparison menu, press any key");
                    ReadKey(true);
                    RunComparasive();
                    break;
                case 1:

                    break;
                case 2:
                    RunMainMenu();
                    break;
                case 3:
                    Exit();
                    break;
                default:
                    break;
            }
        }

        public void Exit()
        {
            WriteLine("\nPress any key to exit...");
            ReadKey(true);
            Environment.Exit(0);
        }

    }
}
