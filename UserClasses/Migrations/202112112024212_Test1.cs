namespace UserClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Matches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_SteamID = c.Long(nullable: false),
                        MatchID = c.Long(nullable: false),
                        StartTime = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DetailsData = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_SteamID, cascadeDelete: true)
                .Index(t => t.User_SteamID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        SteamID = c.Long(nullable: false),
                        CommunityVisible = c.Int(nullable: false),
                        CommentPermission = c.Int(nullable: false),
                        ProfileUrl = c.String(),
                        Avarat = c.String(),
                        LastLogOff = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeCreated = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RealName = c.String(),
                        ProfileName = c.String(),
                        Password = c.String(maxLength: 16),
                        HashedPassword = c.String(),
                        LastTimeMatchesRefreshed = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SteamID)
                .Index(t => t.SteamID, unique: true, name: "UX_Steam_ID");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Matches", "User_SteamID", "dbo.Users");
            DropIndex("dbo.Users", "UX_Steam_ID");
            DropIndex("dbo.Matches", new[] { "User_SteamID" });
            DropTable("dbo.Users");
            DropTable("dbo.Matches");
        }
    }
}
