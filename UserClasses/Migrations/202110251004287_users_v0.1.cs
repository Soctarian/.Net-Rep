namespace UserClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class users_v01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UniqID = c.Int(nullable: false, identity: true),
                        SteamID = c.Long(nullable: false),
                        CommunityVisible = c.Int(nullable: false),
                        CommentPermission = c.Int(nullable: false),
                        ProfileUrl = c.String(),
                        Avarat = c.String(),
                        LastLogOff = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TimeCreated = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RealName = c.String(),
                        ProfileName = c.String(),
                    })
                .PrimaryKey(t => t.UniqID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
