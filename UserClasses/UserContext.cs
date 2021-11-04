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
        { }

        public DbSet<User> Users { get; set; }
    }

    public class User
    {
        [MaxLength(17),MinLength(17)]
        [Index("UX_Steam_ID", IsUnique = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
     // public int UniqID { get; set; }
        public long SteamID { get; set; }
        public int CommunityVisible { get; set; }
        public int CommentPermission { get; set; }
        public string ProfileUrl { get; set; }
        public string Avarat { get; set; }
        public decimal LastLogOff { get; set; }
        public decimal TimeCreated { get; set; }
        public string RealName { get; set; }
        public string ProfileName { get; set; }

        [MaxLength(20), MinLength(6)]
        public string Password { get; set; }

        public string HashedPassword { get; set; }

        public List<Matches> Matches { get; set; }

    }
    public class Matches
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long MatchID { get; set; }
        public decimal StartTime { get; set; }
    }

}
