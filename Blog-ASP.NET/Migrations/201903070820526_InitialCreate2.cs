namespace Blog_ASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CommitLists", "Account", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.TextLists", "Account", c => c.String(nullable: false, maxLength: 8));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TextLists", "Account", c => c.String(nullable: false, maxLength: 12));
            AlterColumn("dbo.CommitLists", "Account", c => c.String(nullable: false));
        }
    }
}
