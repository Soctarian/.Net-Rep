namespace UserClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewKeyforMatches : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Matches", "User_SteamID", "dbo.Users");
            DropPrimaryKey("dbo.Matches");
            AddColumn("dbo.Matches", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Matches", "Id");
            AddForeignKey("dbo.Matches", "User_SteamID", "dbo.Users", "SteamID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Matches", "User_SteamID", "dbo.Users");
            DropPrimaryKey("dbo.Matches");
            DropColumn("dbo.Matches", "Id");
            AddPrimaryKey("dbo.Matches", "User_SteamID");
            AddForeignKey("dbo.Matches", "User_SteamID", "dbo.Users", "SteamID");
        }
    }
}
