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
        public GetUserInfo getuserinfo = new GetUserInfo();
        public RunMenu(decimal profileID32)
        {
            this.profileID32 = profileID32;
        }
        public RunMenu() { }

        public void LoginScreen()
        {
            RunLoginWindow();
        }

        public void StartMenu(User user)
        {
            RunMainMenu(user);
        }

        public void RunLoginWindow()
        {
            string promt = "Sign up, or sign in";
            string[] options = { "Sign up", "Sign in", "Exit" };
            KeyboardMenu loginMenu = new KeyboardMenu(promt, options);
            int SelectedIndex = loginMenu.Run();

            switch (SelectedIndex)
            {
                case 0:
                    long SteamID;
                    string Password, ConfirmPassword;
                    do
                    {
                        Clear();
                        Write("\nInput your SteamID: ");
                        SteamID = Convert.ToInt64(ReadLine());
                        Write("\nInput password: ");
                        Password = ReadLine();
                        Write("\nConfirm your password: ");
                        ConfirmPassword = ReadLine();
                        if (!PasswordEquality(Password, ConfirmPassword)) WriteLine("Password missmatch!\nPress any key");

                    } while (!PasswordEquality(Password, ConfirmPassword));
                    AddUser.RegisterUser(SteamID, Password);
                    WriteLine("Succesfully registrated!");
                    RunLoginWindow();
                    break;
                case 1:

                    do {
                        Clear();
                        Write("\nInput your SteamID: ");
                        SteamID = Convert.ToInt64(ReadLine());
                        Write("\nInput password: ");
                        Password = ReadLine();
                        if(!AddUser.CheckUser(SteamID, Password))
                        {
                            WriteLine("Your password is incorrect!");
                            WriteLine("Press R, if you want create an account or try again");
                            ConsoleKeyInfo keyInfo = ReadKey(true);
                            ConsoleKey keyPressed = keyInfo.Key;
                            if (keyPressed == ConsoleKey.R)
                            {
                                RunLoginWindow();
                            }
                        }
                    } while (!AddUser.CheckUser(SteamID, Password));

                    var task = this.getuserinfo.GetDetailsListFromDBAsync(SteamID);
                    StartMenu(AddUser.GetUser(SteamID));
                    break;
                case 2:
                    Exit();
                    break;
                default:
                    break;
            }

        }

        public void RunMainMenu(User user)
        {
            string promt = $"{user.ProfileName}, welcome to my .Net project!";
            string[] options = { "Get match statistic", "Get statistic for hero", "Comparasive", "Exit" };
            KeyboardMenu mainMenu = new KeyboardMenu(promt, options);
            int SelectedIndex = mainMenu.Run();
            switch (SelectedIndex)
            {
                case 0:
                    RunInputMatchStatistic(user);
                    break;
                case 1:
                    GetHeroStats(user, this.getuserinfo);
                    break;
                case 2:
                    RunComparasive(user);
                    break;
                case 3:
                    Exit();
                    break;
                default:
                    break;
            }

        }

        private void GetHeroStats(User user, GetUserInfo getuserinfo)
        {
            GetHeroStats inputHeroStatsInfo = new GetHeroStats();

            Write("Input hero name: ");
            var hero = ReadLine();
            inputHeroStatsInfo.GetHStats(user.SteamID, hero, getuserinfo);

            WriteLine("If you want to back to the statistic menu, press any key");
            ReadKey(true);
            RunMainMenu(user);
        }

        public void RunInputMatchStatistic(User user)
        {
            OutputMatchInfo inputMatchInfo = new OutputMatchInfo();
            string promt = "Choose what kind of statistic you wanna get: ";
            string[] options = { "Quick match statistic", "Full match statistic", "Back to main menu", "Exit" };
            KeyboardMenu matchStatisticMenu = new KeyboardMenu(promt, options);
            int selectedIndex = matchStatisticMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    Write("\nInput number of matches: ");
                    var countMathces = Convert.ToInt32(ReadLine());
                    inputMatchInfo.OutputQuickMatchStatistic(countMathces, Deciphers.ConvertToSteamID32(user.SteamID));

                    WriteLine("If you want to return to the match statistic menu, press any key");
                    ReadKey(true);
                    RunInputMatchStatistic(user);
                    break;
                case 1:
                    Write("Input match ID: ");
                    var matchID = Convert.ToDecimal(ReadLine());
                    inputMatchInfo.OutputFullkMatchStatistic(matchID);

                    WriteLine("Press y, if you want to get statistic for exact hero");
                    ConsoleKeyInfo keyInfo = ReadKey(true);
                    ConsoleKey keyPressed = keyInfo.Key;
                    if (keyPressed == ConsoleKey.Y)
                    {
                        RunStatisticForHeroInMatch(matchID, user);
                    }
                    else
                    {
                        WriteLine("If you want to return to the match statistic menu, press any key");
                        ReadKey(true);
                        RunInputMatchStatistic(user);
                    }

                    break;
                case 2:
                    RunMainMenu(user);
                    break;
                case 3:
                    Exit();
                    break;
                default:
                    break;
            }

        }

        public void RunStatisticForHeroInMatch(decimal matchID, User user)
        {
            OutputMatchInfo inputMatchInfo = new OutputMatchInfo();

            Write("Input hero name: ");
            var hero = ReadLine();
            inputMatchInfo.OutputDetailMatchStatistic(hero, matchID);

            WriteLine("If you want to back to the statistic menu, press any key");
            ReadKey(true);
            RunInputMatchStatistic(user);

        }


        public void RunComparasive(User user)
        {
            string promt = "Choose what kind of comparasive you wanna do";
            string[] options = { "Time played comparasive", "Winrate comparasive", "Back to main menu", "Exit" };
            KeyboardMenu сomparasiveMenu = new KeyboardMenu(promt, options);
            int selectedIndex = сomparasiveMenu.Run();
            switch (selectedIndex)
            {
                case 0:
                    Write("Enter the id of the player with whom you want to compare time spent in the game: ");
                    var secondID = Convert.ToDecimal(ReadLine());
                    Write("Enter the number of days which you want to compare time for: ");
                    var time = Convert.ToInt32(ReadLine());
                    PlayerComparison comaprison = new PlayerComparison(Deciphers.ConvertToSteamID32(user.SteamID), secondID, time);
                    OutputComparisonInfo.InputTimeComparison(comaprison.TimeComparison(), time);

                    WriteLine("\nIf you want to back to the comparison menu, press any key");
                    ReadKey(true);
                    RunComparasive(user);
                    break;
                case 1:
                    Write("Enter the id of the player with whom you want to compare match results in the game: ");
                    secondID = Convert.ToDecimal(ReadLine());
                    Write("Enter the number of days which you want to compare time for: ");
                    time = Convert.ToInt32(ReadLine());
                    comaprison = new PlayerComparison(Deciphers.ConvertToSteamID32(user.SteamID), secondID, time);
                    OutputComparisonInfo.InputWinrateComparison(comaprison.WinRateAndRankComparison(), time);

                    WriteLine("\nIf you want to back to the comparison menu, press any key");
                    ReadKey(true);
                    RunComparasive(user);
                    break;
                case 2:
                    RunMainMenu(user);
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
        public bool PasswordEquality(string Password, string ConfirmPassword) => Password == ConfirmPassword ? true : false;

    }
}
