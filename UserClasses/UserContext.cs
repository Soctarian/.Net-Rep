using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserClasses
{
    public class UserContext : DbContext
    {
        public UserContext()
          : base("Project")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UserContext, UserClasses.Migrations.Configuration>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Matches> Matches { get; set; }
    }

    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Index("UX_Steam_ID", IsUnique = true)]
        [Key]
        public long SteamID { get; set; }
        public int CommunityVisible { get; set; }
        public int CommentPermission { get; set; }
        public string ProfileUrl { get; set; }
        public string Avarat { get; set; }
        public decimal LastLogOff { get; set; }
        public decimal TimeCreated { get; set; }
        public string RealName { get; set; }
        public string ProfileName { get; set; }

        [MaxLength(16), MinLength(6)]
        public string Password { get; set; }

        public string HashedPassword { get; set; }
        public decimal LastTimeMatchesRefreshed { get; set; }
        public List<Matches> Matches { get; set; }

    }
    public class Matches
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public long User_SteamID { get; set; }
        
        public long MatchID { get; set; }
        public decimal StartTime { get; set; }
        public string DetailsData { get; set; }

        [ForeignKey("User_SteamID")]
        public User User { get; set; }
    }

}
