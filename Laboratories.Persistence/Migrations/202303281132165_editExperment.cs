namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editExperment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Experiments", "expr_teachId", c => c.Int(nullable: false));
            CreateIndex("dbo.Experiments", "expr_teachId");
            AddForeignKey("dbo.Experiments", "expr_teachId", "dbo.Teachers", "Id", cascadeDelete: true);
            DropColumn("dbo.Experiments", "expr_teacherName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Experiments", "expr_teacherName", c => c.String());
            DropForeignKey("dbo.Experiments", "expr_teachId", "dbo.Teachers");
            DropIndex("dbo.Experiments", new[] { "expr_teachId" });
            DropColumn("dbo.Experiments", "expr_teachId");
        }
    }
}
