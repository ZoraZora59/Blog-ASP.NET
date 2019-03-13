namespace NewBeeBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(maxLength: 16),
                        Password = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TextLists",
                c => new
                    {
                        TextID = c.Int(nullable: false, identity: true),
                        TextTitle = c.String(nullable: false, maxLength: 40),
                        Text = c.String(nullable: false),
                        Hot = c.Int(nullable: false),
                        Attachment = c.String(),
                        CategoryName = c.String(maxLength: 12),
                        Account = c.String(nullable: false, maxLength: 8),
                        TextChangeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TextID);
            
            CreateTable(
                "dbo.CommitLists",
                c => new
                    {
                        CommitID = c.Int(nullable: false, identity: true),
                        TextID = c.Int(nullable: false),
                        Account = c.String(nullable: false, maxLength: 8),
                        CommitText = c.String(nullable: false, maxLength: 100),
                        CommitChangeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommitID);
            
            CreateTable(
                "dbo.CommitListTextLists",
                c => new
                    {
                        CommitList_CommitID = c.Int(nullable: false),
                        TextList_TextID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CommitList_CommitID, t.TextList_TextID })
                .ForeignKey("dbo.CommitLists", t => t.CommitList_CommitID, cascadeDelete: true)
                .ForeignKey("dbo.TextLists", t => t.TextList_TextID, cascadeDelete: true)
                .Index(t => t.CommitList_CommitID)
                .Index(t => t.TextList_TextID);
            
            CreateTable(
                "dbo.TextListUsers",
                c => new
                    {
                        TextList_TextID = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TextList_TextID, t.User_Id })
                .ForeignKey("dbo.TextLists", t => t.TextList_TextID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.TextList_TextID)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TextListUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.TextListUsers", "TextList_TextID", "dbo.TextLists");
            DropForeignKey("dbo.CommitListTextLists", "TextList_TextID", "dbo.TextLists");
            DropForeignKey("dbo.CommitListTextLists", "CommitList_CommitID", "dbo.CommitLists");
            DropIndex("dbo.TextListUsers", new[] { "User_Id" });
            DropIndex("dbo.TextListUsers", new[] { "TextList_TextID" });
            DropIndex("dbo.CommitListTextLists", new[] { "TextList_TextID" });
            DropIndex("dbo.CommitListTextLists", new[] { "CommitList_CommitID" });
            DropTable("dbo.TextListUsers");
            DropTable("dbo.CommitListTextLists");
            DropTable("dbo.CommitLists");
            DropTable("dbo.TextLists");
            DropTable("dbo.Users");
        }
    }
}
