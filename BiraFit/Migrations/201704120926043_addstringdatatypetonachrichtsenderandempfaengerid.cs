namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addstringdatatypetonachrichtsenderandempfaengerid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Konversation", "Sportler_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Konversation", "PersonalTrainer_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Nachricht", "Sender_Id", c => c.String(nullable: false));
            AlterColumn("dbo.Nachricht", "Empfaenger_Id", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Nachricht", "Empfaenger_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Nachricht", "Sender_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Konversation", "PersonalTrainer_Id", c => c.String());
            AlterColumn("dbo.Konversation", "Sportler_Id", c => c.String());
        }
    }
}
