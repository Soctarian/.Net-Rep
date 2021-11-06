namespace UserClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedValidation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Password", c => c.String(maxLength: 20));
            AddColumn("dbo.Users", "HashedPassword", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "HashedPassword");
            DropColumn("dbo.Users", "Password");
        }
    }
}
