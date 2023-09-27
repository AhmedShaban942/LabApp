namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddeparttoCourse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "cors_exprNum", c => c.Int(nullable: false));
            AddColumn("dbo.Courses", "cors_term", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "cors_term");
            DropColumn("dbo.Courses", "cors_exprNum");
        }
    }
}
