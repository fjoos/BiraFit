namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Angebot",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TrainerID = c.Int(nullable: false),
                        Beschreibung = c.String(nullable: false),
                        Preis = c.Int(nullable: false),
                        Datum = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bedarf", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Bedarf",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Open = c.Boolean(nullable: false),
                        Titel = c.String(nullable: false),
                        Beschreibung = c.String(nullable: false),
                        Preis = c.Int(nullable: false),
                        Ort = c.String(nullable: false),
                        Datum = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sportler", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Sportler",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Konversation",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sportler", t => t.Id)
                .ForeignKey("dbo.Personaltrainer", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Personaltrainer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Bewertung = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Nachricht",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        SenderID = c.Int(nullable: false),
                        TrainerID = c.Int(nullable: false),
                        Datum = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Konversation", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Nachricht", "Id", "dbo.Konversation");
            DropForeignKey("dbo.Konversation", "Id", "dbo.Personaltrainer");
            DropForeignKey("dbo.Konversation", "Id", "dbo.Sportler");
            DropForeignKey("dbo.Angebot", "Id", "dbo.Bedarf");
            DropForeignKey("dbo.Bedarf", "Id", "dbo.Sportler");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Nachricht", new[] { "Id" });
            DropIndex("dbo.Konversation", new[] { "Id" });
            DropIndex("dbo.Bedarf", new[] { "Id" });
            DropIndex("dbo.Angebot", new[] { "Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Nachricht");
            DropTable("dbo.Personaltrainer");
            DropTable("dbo.Konversation");
            DropTable("dbo.Sportler");
            DropTable("dbo.Bedarf");
            DropTable("dbo.Angebot");
        }
    }
}
