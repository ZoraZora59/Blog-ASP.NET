namespace Blog_ASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CheckPassword", c => c.String(maxLength: 11));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CheckPassword");
        }
    }
}
