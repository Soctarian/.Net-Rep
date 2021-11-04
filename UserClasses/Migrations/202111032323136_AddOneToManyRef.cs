namespace UserClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOneToManyRef : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Users");
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        MatchID = c.Long(nullable: false, identity: true),
                        StartTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        User_SteamID = c.Long(),
                    })
                .PrimaryKey(t => t.MatchID)
                .ForeignKey("dbo.Users", t => t.User_SteamID)
                .Index(t => t.User_SteamID);
            
            AlterColumn("dbo.Users", "SteamID", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Users", "SteamID");
            CreateIndex("dbo.Users", "SteamID", unique: true, name: "UX_Steam_ID");
            DropColumn("dbo.Users", "UniqID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "UniqID", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.Matches", "User_SteamID", "dbo.Users");
            DropIndex("dbo.Matches", new[] { "User_SteamID" });
            DropIndex("dbo.Users", "UX_Steam_ID");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "SteamID", c => c.Long(nullable: false));
            DropTable("dbo.Matches");
            AddPrimaryKey("dbo.Users", "UniqID");
        }
    }
}
