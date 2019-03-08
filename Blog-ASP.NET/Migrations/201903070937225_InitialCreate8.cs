namespace Blog_ASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate8 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "CheckPassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CheckPassword", c => c.String(maxLength: 11));
        }
    }
}
