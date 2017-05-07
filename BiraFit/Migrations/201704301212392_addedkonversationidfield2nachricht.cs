namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class addedkonversationidfield2nachricht : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Nachricht", "Konversation_Id", "dbo.Konversation");
            DropIndex("dbo.Nachricht", new[] {"Konversation_Id"});
            AddColumn("dbo.Nachricht", "Konversation_Id1", c => c.Int());
            AlterColumn("dbo.Nachricht", "Konversation_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Nachricht", "Konversation_Id1");
            AddForeignKey("dbo.Nachricht", "Konversation_Id1", "dbo.Konversation", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Nachricht", "Konversation_Id1", "dbo.Konversation");
            DropIndex("dbo.Nachricht", new[] {"Konversation_Id1"});
            AlterColumn("dbo.Nachricht", "Konversation_Id", c => c.Int());
            DropColumn("dbo.Nachricht", "Konversation_Id1");
            CreateIndex("dbo.Nachricht", "Konversation_Id");
            AddForeignKey("dbo.Nachricht", "Konversation_Id", "dbo.Konversation", "Id");
        }
    }
}