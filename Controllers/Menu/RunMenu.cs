using System;
using System.Collections.Generic;
using System.Text;
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
                    break;
                case 3:
                    break;
                case 4:
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
                    RunStatisticForHeroInMatch(matchID);

                    WriteLine("If you want to return to the match statistic menu, press any key");
                    ReadKey(true);
                    RunInputMatchStatistic();
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
            string promt = "Check statistic for exact hero?";
            string[] options = { "Yes", "Exit" };
            KeyboardMenu heroStatisticMenu = new KeyboardMenu(promt, options);
            int selectedIndex = heroStatisticMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    Write("Input hero name: "); 
                    var hero = ReadLine();
                    inputMatchInfo.InputDetailMatchStatistic(hero, matchID);

                    WriteLine("If you want to back to the statistic menu, press any key");
                    ReadKey(true);
                    RunInputMatchStatistic();
                    break;
                case 1:
                    ReadKey(true);
                    Exit();
                    break;
                default:
                    break;
            }
        }
        public void RunAddUserToDB()
        {

        }
        public void RunComparasive()
        {

        }

        public void Exit()
        {
            WriteLine("\nPress any key to exit...");
            ReadKey(true);
            Environment.Exit(0);
        }

    }
}
