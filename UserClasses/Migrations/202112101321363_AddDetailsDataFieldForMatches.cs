namespace UserClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDetailsDataFieldForMatches : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Matches", "DetailsData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Matches", "DetailsData");
        }
    }
}
