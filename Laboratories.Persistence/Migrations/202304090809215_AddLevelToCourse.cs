namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLevelToCourse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "cors_level", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "cors_level");
        }
    }
}
