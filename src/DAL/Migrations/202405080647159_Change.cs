namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Roles", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Roles", "Description", c => c.String());
        }
    }
}
