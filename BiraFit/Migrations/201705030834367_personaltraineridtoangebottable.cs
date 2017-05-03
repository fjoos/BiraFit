namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class personaltraineridtoangebottable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Angebot", "PersonalTrainer_Id", "dbo.Personaltrainer");
            DropIndex("dbo.Angebot", new[] { "PersonalTrainer_Id" });
            AddColumn("dbo.Angebot", "PersonalTrainer_Id1", c => c.Int());
            AlterColumn("dbo.Angebot", "PersonalTrainer_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Angebot", "PersonalTrainer_Id1");
            AddForeignKey("dbo.Angebot", "PersonalTrainer_Id1", "dbo.Personaltrainer", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Angebot", "PersonalTrainer_Id1", "dbo.Personaltrainer");
            DropIndex("dbo.Angebot", new[] { "PersonalTrainer_Id1" });
            AlterColumn("dbo.Angebot", "PersonalTrainer_Id", c => c.Int());
            DropColumn("dbo.Angebot", "PersonalTrainer_Id1");
            CreateIndex("dbo.Angebot", "PersonalTrainer_Id");
            AddForeignKey("dbo.Angebot", "PersonalTrainer_Id", "dbo.Personaltrainer", "Id");
        }
    }
}
