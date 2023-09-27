namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addexpermentsandCourse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        cors_arName = c.String(nullable: false),
                        cors_enName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Experiments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        expr_arName = c.String(),
                        expr_enName = c.String(),
                        expr_tpye = c.String(),
                        expr_page = c.Int(nullable: false),
                        expr_chapter = c.Int(nullable: false),
                        expr_corsId = c.Int(nullable: false),
                        expr_tools = c.String(),
                        expr_state = c.Int(nullable: false),
                        expr_teacherName = c.String(),
                        expr_teacherSignature = c.String(),
                        expr_year = c.Int(nullable: false),
                        AddedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        AddedBy = c.Int(),
                        ModifiedBy = c.Int(),
                        IsAvtive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.expr_corsId, cascadeDelete: true)
                .Index(t => t.expr_corsId);
            
            AddColumn("dbo.Items", "itm_year", c => c.Int(nullable: false));
            AddColumn("dbo.ScheduleDets", "schd_lessonNum4", c => c.Int(nullable: false));
            AddColumn("dbo.ScheduleDets", "schd_subject4", c => c.Int(nullable: false));
            AddColumn("dbo.ScheduleDets", "schd_lessonNum5", c => c.Int(nullable: false));
            AddColumn("dbo.ScheduleDets", "schd_subject5", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Experiments", "expr_corsId", "dbo.Courses");
            DropIndex("dbo.Experiments", new[] { "expr_corsId" });
            DropColumn("dbo.ScheduleDets", "schd_subject5");
            DropColumn("dbo.ScheduleDets", "schd_lessonNum5");
            DropColumn("dbo.ScheduleDets", "schd_subject4");
            DropColumn("dbo.ScheduleDets", "schd_lessonNum4");
            DropColumn("dbo.Items", "itm_year");
            DropTable("dbo.Experiments");
            DropTable("dbo.Courses");
        }
    }
}
