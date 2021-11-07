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
        public static void AttachUser(long UserSteamID)
        {
            GetUserInfo getinfo = new GetUserInfo();
            getinfo.DeterminatePlayerInfo(UserSteamID);

            using (UserContext db = new UserContext())
            {
                User user = new User {
                    SteamID = (long)UserSteamID,
                    CommunityVisible = getinfo.CommunityVisible,
                    CommentPermission = getinfo.CommentPermission,
                    ProfileUrl = getinfo.ProfileUrl,
                    Avarat = getinfo.Avatar,
                    LastLogOff = getinfo.LastLogOff,
                    TimeCreated = getinfo.TimeCreated,
                    RealName = getinfo.RealName,
                    ProfileName = getinfo.Login
                };
                db.Users.Add(user);
                db.SaveChanges();

            }
        }

        public static void RegisterUser(long UserSteamID, string Password)
        {
                GetUserInfo getinfo = new GetUserInfo();
                getinfo.DeterminatePlayerInfo(UserSteamID);

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

        public static bool CheckUser(long SteamID, string Password)
        {
            bool isLogged = false;
            var user = new UserContext();
            if (Deciphers.VerifyHashedPassword(user.Users.Find(SteamID).HashedPassword, Password)) isLogged = true; 
            return isLogged;
        }
        public static User GetUser(long SteamID)
        {
            var user = new UserContext();
            return user.Users.Find(SteamID);
        }

    }
}
