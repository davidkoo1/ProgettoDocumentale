namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Roles", t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 7),
                        Password = c.String(),
                        Email = c.String(nullable: false, maxLength: 150),
                        IsEnabled = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Surname = c.String(nullable: false, maxLength: 100),
                        Patronymic = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.Email, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Roles", new[] { "Name" });
            DropTable("dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
        }
    }
}
