namespace UserClasses.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordMaxLengthChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(maxLength: 16));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String(maxLength: 20));
        }
    }
}
