namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAspNetUsersTable : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE AspNetUsers ADD Name VARCHAR(50) NOT NULL");
            Sql("ALTER TABLE AspNetUsers ADD Vorname VARCHAR(50) NOT NULL");
            Sql("ALTER TABLE AspNetUsers ADD Adresse VARCHAR(250) NOT NULL");
            Sql("ALTER TABLE AspNetUsers ADD ProfilBild VARCHAR(250) NULL");
            Sql("ALTER TABLE AspNetUsers ADD Aktiv BIT NOT NULL");
            Sql("ALTER TABLE AspNetUsers ADD AnmeldeDatum DateTime NOT NULL");
        }
        
        public override void Down()
        {
        }
    }
}
