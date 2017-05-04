namespace BiraFit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class initialcreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Angebot",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Beschreibung = c.String(nullable: false),
                        Preis = c.Int(nullable: false),
                        Datum = c.DateTime(nullable: false),
                        Bedarf_Id = c.Int(nullable: false),
                        Bedarf_Id1 = c.Int(),
                        PersonalTrainer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bedarf", t => t.Bedarf_Id1)
                .ForeignKey("dbo.Personaltrainer", t => t.PersonalTrainer_Id)
                .Index(t => t.Bedarf_Id1)
                .Index(t => t.PersonalTrainer_Id);

            CreateTable(
                    "dbo.Bedarf",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OpenBedarf = c.Boolean(nullable: false),
                        Titel = c.String(nullable: false),
                        Beschreibung = c.String(nullable: false),
                        Preis = c.Int(nullable: false),
                        Ort = c.String(nullable: false),
                        Datum = c.DateTime(nullable: false),
                        Sportler_Id = c.Int(nullable: false),
                        Sportler_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sportler", t => t.Sportler_Id1)
                .Index(t => t.Sportler_Id1);

            CreateTable(
                    "dbo.Konversation",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sportler_Id = c.Int(nullable: false),
                        PersonalTrainer_Id = c.Int(nullable: false),
                        PersonalTrainer_Id1 = c.Int(),
                        Sportler_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Personaltrainer", t => t.PersonalTrainer_Id1)
                .ForeignKey("dbo.Sportler", t => t.Sportler_Id1)
                .Index(t => t.PersonalTrainer_Id1)
                .Index(t => t.Sportler_Id1);

            CreateTable(
                    "dbo.Nachricht",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        Sender_Id = c.String(nullable: false),
                        Empfaenger_Id = c.String(nullable: false),
                        Datum = c.DateTime(nullable: false),
                        Konversation_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Konversation", t => t.Konversation_Id)
                .Index(t => t.Konversation_Id);

            CreateTable(
                    "dbo.Personaltrainer",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Bewertung = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);

            CreateTable(
                    "dbo.AspNetUsers",
                    c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AnmeldeDatum = c.DateTime(nullable: false),
                        Name = c.String(nullable: false),
                        Vorname = c.String(nullable: false),
                        Adresse = c.String(nullable: false),
                        ProfilBild = c.String(),
                        Aktiv = c.Int(nullable: false),
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
                .PrimaryKey(t => new {t.LoginProvider, t.ProviderKey, t.UserId})
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                    "dbo.AspNetUserRoles",
                    c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new {t.UserId, t.RoleId})
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

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
                    "dbo.Sportler",
                    c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Sportler", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Konversation", "Sportler_Id1", "dbo.Sportler");
            DropForeignKey("dbo.Bedarf", "Sportler_Id1", "dbo.Sportler");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Personaltrainer", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Konversation", "PersonalTrainer_Id1", "dbo.Personaltrainer");
            DropForeignKey("dbo.Angebot", "PersonalTrainer_Id", "dbo.Personaltrainer");
            DropForeignKey("dbo.Nachricht", "Konversation_Id", "dbo.Konversation");
            DropForeignKey("dbo.Angebot", "Bedarf_Id1", "dbo.Bedarf");
            DropIndex("dbo.Sportler", new[] {"User_Id"});
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] {"RoleId"});
            DropIndex("dbo.AspNetUserRoles", new[] {"UserId"});
            DropIndex("dbo.AspNetUserLogins", new[] {"UserId"});
            DropIndex("dbo.AspNetUserClaims", new[] {"UserId"});
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Personaltrainer", new[] {"User_Id"});
            DropIndex("dbo.Nachricht", new[] {"Konversation_Id"});
            DropIndex("dbo.Konversation", new[] {"Sportler_Id1"});
            DropIndex("dbo.Konversation", new[] {"PersonalTrainer_Id1"});
            DropIndex("dbo.Bedarf", new[] {"Sportler_Id1"});
            DropIndex("dbo.Angebot", new[] {"PersonalTrainer_Id"});
            DropIndex("dbo.Angebot", new[] {"Bedarf_Id1"});
            DropTable("dbo.Sportler");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Personaltrainer");
            DropTable("dbo.Nachricht");
            DropTable("dbo.Konversation");
            DropTable("dbo.Bedarf");
            DropTable("dbo.Angebot");
        }
    }
}