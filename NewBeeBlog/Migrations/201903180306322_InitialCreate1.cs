namespace NewBeeBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TextLists", "Attachment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TextLists", "Attachment", c => c.String());
        }
    }
}
