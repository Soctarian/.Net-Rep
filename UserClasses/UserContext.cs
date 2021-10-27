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
          : base("DbConnection")
        { }

        public DbSet<User> Users { get; set; }
    }

    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int UniqID { get; set; }
        public long SteamID { get; set; }
        public int CommunityVisible { get; set; }
        public int CommentPermission { get; set; }
        public string ProfileUrl { get; set; }
        public string Avarat { get; set; }
        public decimal LastLogOff { get; set; }
        public decimal TimeCreated { get; set; }
        public string RealName { get; set; }
        public string ProfileName { get; set; }

    }

}
