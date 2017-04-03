namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AngebotBedarfIDFieldadd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Angebot", "Bedarf_Id", "dbo.Bedarf");
            DropIndex("dbo.Angebot", new[] { "Bedarf_Id" });
            AddColumn("dbo.Angebot", "Bedarf_Id1", c => c.Int());
            AlterColumn("dbo.Angebot", "Bedarf_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Angebot", "Bedarf_Id1");
            AddForeignKey("dbo.Angebot", "Bedarf_Id1", "dbo.Bedarf", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Angebot", "Bedarf_Id1", "dbo.Bedarf");
            DropIndex("dbo.Angebot", new[] { "Bedarf_Id1" });
            AlterColumn("dbo.Angebot", "Bedarf_Id", c => c.Int());
            DropColumn("dbo.Angebot", "Bedarf_Id1");
            CreateIndex("dbo.Angebot", "Bedarf_Id");
            AddForeignKey("dbo.Angebot", "Bedarf_Id", "dbo.Bedarf", "Id");
        }
    }
}
