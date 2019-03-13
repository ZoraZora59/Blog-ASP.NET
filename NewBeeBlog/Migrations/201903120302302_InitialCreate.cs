namespace NewBeeBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CommitListTextLists", newName: "TextListCommitLists");
            DropPrimaryKey("dbo.TextListCommitLists");
            AddPrimaryKey("dbo.TextListCommitLists", new[] { "TextList_TextID", "CommitList_CommitID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TextListCommitLists");
            AddPrimaryKey("dbo.TextListCommitLists", new[] { "CommitList_CommitID", "TextList_TextID" });
            RenameTable(name: "dbo.TextListCommitLists", newName: "CommitListTextLists");
        }
    }
}
