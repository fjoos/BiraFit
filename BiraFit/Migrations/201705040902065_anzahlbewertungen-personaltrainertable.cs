namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anzahlbewertungenpersonaltrainertable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Personaltrainer", "AnzahlBewertungen", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Personaltrainer", "AnzahlBewertungen");
        }
    }
}
