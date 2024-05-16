namespace DAL.Context.Persistance
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentsTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        SavedPath = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                        AdditionalInfo = c.String(maxLength: 1000),
                        GroupingDate = c.DateTime(nullable: false),
                        InstitutionId = c.Int(),
                        UserId = c.Int(nullable: false),
                        TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentTypes", t => t.TypeId, cascadeDelete: true)
                .ForeignKey("dbo.Institutions", t => t.InstitutionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.InstitutionId)
                .Index(t => t.UserId)
                .Index(t => t.TypeId);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 255),
                        TypeDscr = c.String(maxLength: 1000),
                        IsMacro = c.Boolean(nullable: false),
                        IsDateGrouped = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentTypeHierarchies",
                c => new
                    {
                        IdMacro = c.Int(nullable: false),
                        IdMicro = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdMacro, t.IdMicro })
                .ForeignKey("dbo.DocumentTypes", t => t.IdMacro)
                .ForeignKey("dbo.DocumentTypes", t => t.IdMicro)
                .Index(t => t.IdMacro)
                .Index(t => t.IdMicro);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "UserId", "dbo.Users");
            DropForeignKey("dbo.Documents", "InstitutionId", "dbo.Institutions");
            DropForeignKey("dbo.Documents", "TypeId", "dbo.DocumentTypes");
            DropForeignKey("dbo.DocumentTypeHierarchies", "IdMicro", "dbo.DocumentTypes");
            DropForeignKey("dbo.DocumentTypeHierarchies", "IdMacro", "dbo.DocumentTypes");
            DropIndex("dbo.DocumentTypeHierarchies", new[] { "IdMicro" });
            DropIndex("dbo.DocumentTypeHierarchies", new[] { "IdMacro" });
            DropIndex("dbo.Documents", new[] { "TypeId" });
            DropIndex("dbo.Documents", new[] { "UserId" });
            DropIndex("dbo.Documents", new[] { "InstitutionId" });
            DropTable("dbo.DocumentTypeHierarchies");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.Documents");
        }
    }
}
