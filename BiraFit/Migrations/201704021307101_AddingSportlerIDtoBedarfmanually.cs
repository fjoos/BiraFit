namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingSportlerIDtoBedarfmanually : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bedarf", "Sportler_Id", "dbo.Sportler");
            DropIndex("dbo.Bedarf", new[] { "Sportler_Id" });
            AddColumn("dbo.Bedarf", "Sportler_Id1", c => c.Int());
            AlterColumn("dbo.Bedarf", "Sportler_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Bedarf", "Sportler_Id1");
            AddForeignKey("dbo.Bedarf", "Sportler_Id1", "dbo.Sportler", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bedarf", "Sportler_Id1", "dbo.Sportler");
            DropIndex("dbo.Bedarf", new[] { "Sportler_Id1" });
            AlterColumn("dbo.Bedarf", "Sportler_Id", c => c.Int());
            DropColumn("dbo.Bedarf", "Sportler_Id1");
            CreateIndex("dbo.Bedarf", "Sportler_Id");
            AddForeignKey("dbo.Bedarf", "Sportler_Id", "dbo.Sportler", "Id");
        }
    }
}
