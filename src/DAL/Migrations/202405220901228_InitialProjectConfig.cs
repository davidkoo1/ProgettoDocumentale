namespace DAL.Context.Persistance
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialProjectConfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        DateFrom = c.DateTime(nullable: false),
                        DateTill = c.DateTime(nullable: false),
                        AdditionalInfo = c.String(maxLength: 1000),
                        IsActive = c.Boolean(nullable: false),
                        InstitutionId = c.Int(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Institutions", t => t.InstitutionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.InstitutionId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Documents", "ProjectId", c => c.Int());
            CreateIndex("dbo.Documents", "ProjectId");
            AddForeignKey("dbo.Documents", "ProjectId", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "UserId", "dbo.Users");
            DropForeignKey("dbo.Projects", "InstitutionId", "dbo.Institutions");
            DropIndex("dbo.Projects", new[] { "UserId" });
            DropIndex("dbo.Projects", new[] { "InstitutionId" });
            DropIndex("dbo.Documents", new[] { "ProjectId" });
            DropColumn("dbo.Documents", "ProjectId");
            DropTable("dbo.Projects");
        }
    }
}
