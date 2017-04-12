namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class personaltraineridsportleridkonversation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Konversation", "PersonalTrainer_Id", "dbo.Personaltrainer");
            DropForeignKey("dbo.Konversation", "Sportler_Id", "dbo.Sportler");
            DropIndex("dbo.Konversation", new[] { "PersonalTrainer_Id" });
            DropIndex("dbo.Konversation", new[] { "Sportler_Id" });
            AddColumn("dbo.Konversation", "PersonalTrainer_Id1", c => c.Int());
            AddColumn("dbo.Konversation", "Sportler_Id1", c => c.Int());
            AlterColumn("dbo.Konversation", "PersonalTrainer_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Konversation", "Sportler_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Konversation", "PersonalTrainer_Id1");
            CreateIndex("dbo.Konversation", "Sportler_Id1");
            AddForeignKey("dbo.Konversation", "PersonalTrainer_Id1", "dbo.Personaltrainer", "Id");
            AddForeignKey("dbo.Konversation", "Sportler_Id1", "dbo.Sportler", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Konversation", "Sportler_Id1", "dbo.Sportler");
            DropForeignKey("dbo.Konversation", "PersonalTrainer_Id1", "dbo.Personaltrainer");
            DropIndex("dbo.Konversation", new[] { "Sportler_Id1" });
            DropIndex("dbo.Konversation", new[] { "PersonalTrainer_Id1" });
            AlterColumn("dbo.Konversation", "Sportler_Id", c => c.Int());
            AlterColumn("dbo.Konversation", "PersonalTrainer_Id", c => c.Int());
            DropColumn("dbo.Konversation", "Sportler_Id1");
            DropColumn("dbo.Konversation", "PersonalTrainer_Id1");
            CreateIndex("dbo.Konversation", "Sportler_Id");
            CreateIndex("dbo.Konversation", "PersonalTrainer_Id");
            AddForeignKey("dbo.Konversation", "Sportler_Id", "dbo.Sportler", "Id");
            AddForeignKey("dbo.Konversation", "PersonalTrainer_Id", "dbo.Personaltrainer", "Id");
        }
    }
}
