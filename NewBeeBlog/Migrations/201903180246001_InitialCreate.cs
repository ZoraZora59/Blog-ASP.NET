namespace NewBeeBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TextLists", "TextChangeDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TextLists", "TextChangeDate", c => c.DateTime(nullable: false));
        }
    }
}
