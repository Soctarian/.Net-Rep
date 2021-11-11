namespace UserClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeV10 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Matches");
            AddColumn("dbo.Matches", "SteamID", c => c.Long(nullable: false));
            AddColumn("dbo.Users", "LastTimeMatchesRefreshed", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddPrimaryKey("dbo.Matches", "SteamID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Matches");
            DropColumn("dbo.Users", "LastTimeMatchesRefreshed");
            DropColumn("dbo.Matches", "SteamID");
            AddPrimaryKey("dbo.Matches", "MatchID");
        }
    }
}
