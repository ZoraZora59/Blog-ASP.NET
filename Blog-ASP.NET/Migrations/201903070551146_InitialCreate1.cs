namespace Blog_ASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommitLists",
                c => new
                    {
                        CommitID = c.Int(nullable: false, identity: true),
                        TextID = c.Int(nullable: false),
                        Account = c.String(nullable: false),
                        CommitText = c.String(nullable: false, maxLength: 100),
                        CommitChangeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommitID);
            
            CreateTable(
                "dbo.TextLists",
                c => new
                    {
                        TextID = c.Int(nullable: false, identity: true),
                        TextTitle = c.String(nullable: false, maxLength: 40),
                        Tag = c.String(nullable: false, maxLength: 12),
                        Account = c.String(nullable: false, maxLength: 12),
                        Text = c.String(nullable: false, maxLength: 4000),
                        TextChangeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TextID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Account = c.String(nullable: false, maxLength: 8),
                        Password = c.String(nullable: false, maxLength: 11),
                    })
                .PrimaryKey(t => t.Account);
            
            CreateTable(
                "dbo.TextListCommitLists",
                c => new
                    {
                        TextList_TextID = c.Int(nullable: false),
                        CommitList_CommitID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TextList_TextID, t.CommitList_CommitID })
                .ForeignKey("dbo.TextLists", t => t.TextList_TextID, cascadeDelete: true)
                .ForeignKey("dbo.CommitLists", t => t.CommitList_CommitID, cascadeDelete: true)
                .Index(t => t.TextList_TextID)
                .Index(t => t.CommitList_CommitID);
            
            CreateTable(
                "dbo.UserTextLists",
                c => new
                    {
                        User_Account = c.String(nullable: false, maxLength: 8),
                        TextList_TextID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Account, t.TextList_TextID })
                .ForeignKey("dbo.Users", t => t.User_Account, cascadeDelete: true)
                .ForeignKey("dbo.TextLists", t => t.TextList_TextID, cascadeDelete: true)
                .Index(t => t.User_Account)
                .Index(t => t.TextList_TextID);
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Account = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            DropForeignKey("dbo.UserTextLists", "TextList_TextID", "dbo.TextLists");
            DropForeignKey("dbo.UserTextLists", "User_Account", "dbo.Users");
            DropForeignKey("dbo.TextListCommitLists", "CommitList_CommitID", "dbo.CommitLists");
            DropForeignKey("dbo.TextListCommitLists", "TextList_TextID", "dbo.TextLists");
            DropIndex("dbo.UserTextLists", new[] { "TextList_TextID" });
            DropIndex("dbo.UserTextLists", new[] { "User_Account" });
            DropIndex("dbo.TextListCommitLists", new[] { "CommitList_CommitID" });
            DropIndex("dbo.TextListCommitLists", new[] { "TextList_TextID" });
            DropTable("dbo.UserTextLists");
            DropTable("dbo.TextListCommitLists");
            DropTable("dbo.Users");
            DropTable("dbo.TextLists");
            DropTable("dbo.CommitLists");
        }
    }
}
