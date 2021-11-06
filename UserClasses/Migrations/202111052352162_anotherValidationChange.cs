namespace UserClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anotherValidationChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Matches", "User_SteamID", "dbo.Users");
            DropIndex("dbo.Users", "UX_Steam_ID");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "SteamID", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Users", "SteamID");
            CreateIndex("dbo.Users", "SteamID", unique: true, name: "UX_Steam_ID");
            AddForeignKey("dbo.Matches", "User_SteamID", "dbo.Users", "SteamID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Matches", "User_SteamID", "dbo.Users");
            DropIndex("dbo.Users", "UX_Steam_ID");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "SteamID", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Users", "SteamID");
            CreateIndex("dbo.Users", "SteamID", unique: true, name: "UX_Steam_ID");
            AddForeignKey("dbo.Matches", "User_SteamID", "dbo.Users", "SteamID");
        }
    }
}
