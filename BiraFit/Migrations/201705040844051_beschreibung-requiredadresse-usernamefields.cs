namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class beschreibungrequiredadresseusernamefields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Personaltrainer", "Beschreibung", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Adresse", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Adresse", c => c.String(nullable: false));
            DropColumn("dbo.Personaltrainer", "Beschreibung");
        }
    }
}
