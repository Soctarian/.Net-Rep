namespace UserClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMatches : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Matches");
            AlterColumn("dbo.Matches", "MatchID", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Matches", "MatchID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Matches");
            AlterColumn("dbo.Matches", "MatchID", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Matches", "MatchID");
        }
    }
}
