namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IstitutionModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Institutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InstCode = c.String(),
                        Name = c.String(),
                        AdditionalInfo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "IdInstitution", c => c.Int());
            AddColumn("dbo.Users", "Institution_Id", c => c.Int());
            CreateIndex("dbo.Users", "Institution_Id");
            AddForeignKey("dbo.Users", "Institution_Id", "dbo.Institutions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Institution_Id", "dbo.Institutions");
            DropIndex("dbo.Users", new[] { "Institution_Id" });
            DropColumn("dbo.Users", "Institution_Id");
            DropColumn("dbo.Users", "IdInstitution");
            DropTable("dbo.Institutions");
        }
    }
}
