namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IstitutionModelUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Users", "IdInstitution");
            RenameColumn(table: "dbo.Users", name: "Institution_Id", newName: "IdInstitution");
            RenameIndex(table: "dbo.Users", name: "IX_Institution_Id", newName: "IX_IdInstitution");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Users", name: "IX_IdInstitution", newName: "IX_Institution_Id");
            RenameColumn(table: "dbo.Users", name: "IdInstitution", newName: "Institution_Id");
            AddColumn("dbo.Users", "IdInstitution", c => c.Int());
        }
    }
}
