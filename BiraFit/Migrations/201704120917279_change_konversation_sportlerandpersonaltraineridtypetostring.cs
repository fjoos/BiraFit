namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_konversation_sportlerandpersonaltraineridtypetostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Konversation", "Sportler_Id", c => c.String());
            AlterColumn("dbo.Konversation", "PersonalTrainer_Id", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Konversation", "PersonalTrainer_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Konversation", "Sportler_Id", c => c.Int(nullable: false));
        }
    }
}
