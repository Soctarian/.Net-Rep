using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DeserializeObjects;
using Controllers;

namespace UserClasses
{
    public class AddUser
    {
        public static void RegisterUser(long UserSteamID, string Password)
        {
                GetUserInfo getinfo = new GetUserInfo(UserSteamID);
                getinfo.DeterminatePlayerInfo();

                using (UserContext db = new UserContext())
                {
                    User user = new User
                    {
                        SteamID = (long)UserSteamID,
                        CommunityVisible = getinfo.CommunityVisible,
                        CommentPermission = getinfo.CommentPermission,
                        ProfileUrl = getinfo.ProfileUrl,
                        Avarat = getinfo.Avatar,
                        LastLogOff = getinfo.LastLogOff,
                        TimeCreated = getinfo.TimeCreated,
                        RealName = getinfo.RealName,
                        ProfileName = getinfo.Login,
                        Password = Password,
                        HashedPassword = Deciphers.HashPassword(Password)
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
                }
        }

        public static bool CheckUser(string login, string Password)
        {
            bool isLogged = false;
            User findUser = new User();
            using (var user = new UserContext())
            {
                findUser = user.Users.First(user => user.ProfileName == login);
            }
            if (Deciphers.VerifyHashedPassword(findUser.HashedPassword, Password)) isLogged = true;
            return isLogged;
        }
        public static User GetUserByLogin(string login)
        {
            User findUser = new User();
            using (var user = new UserContext())
            {
                findUser = user.Users.First(user => user.ProfileName == login);
            }
            return findUser;
        }
    }
}
