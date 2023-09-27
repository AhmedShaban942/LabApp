namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addExamTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        std_IdentityNumber = c.Long(nullable: false),
                        std_arName = c.String(nullable: false),
                        std_enName = c.String(),
                        std_phon = c.String(),
                        std_email = c.String(),
                        std_address = c.String(),
                        std_department = c.Int(nullable: false),
                        std_level = c.Int(nullable: false),
                        std_levelRecord = c.Int(nullable: false),
                        std_image = c.Binary(),
                        std_schId = c.Int(),
                        AddedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        AddedBy = c.Int(),
                        ModifiedBy = c.Int(),
                        IsAvtive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.std_schId)
                .Index(t => t.std_schId);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        exm_arName = c.String(nullable: false),
                        exm_enName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentGradesDets",
                c => new
                    {
                        stdGD_studentGradesHedId = c.Long(nullable: false),
                        stdGD_studentId = c.Int(nullable: false),
                        stdGD_degree = c.Double(nullable: false),
                        stdGD_rate = c.Double(nullable: false),
                    })
                .PrimaryKey(t => new { t.stdGD_studentGradesHedId, t.stdGD_studentId })
                .ForeignKey("dbo.Students", t => t.stdGD_studentId, cascadeDelete: true)
                .ForeignKey("dbo.StudentGradesHeds", t => t.stdGD_studentGradesHedId, cascadeDelete: true)
                .Index(t => t.stdGD_studentGradesHedId)
                .Index(t => t.stdGD_studentId);
            
            CreateTable(
                "dbo.StudentGradesHeds",
                c => new
                    {
                        stdGH_Id = c.Long(nullable: false, identity: true),
                        stdGH_schId = c.Int(nullable: false),
                        stdGH_studyCourseId = c.Int(nullable: false),
                        stdGH_teacherId = c.Int(nullable: false),
                        stdGH_examId = c.Int(nullable: false),
                        stdGH_Date = c.String(),
                        stdGH_term = c.Int(nullable: false),
                        stdGH_level = c.Int(nullable: false),
                        stdGH_stdRecord = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.stdGH_Id)
                .ForeignKey("dbo.Exams", t => t.stdGH_examId, cascadeDelete: true)
                .ForeignKey("dbo.Schools", t => t.stdGH_schId, cascadeDelete: true)
                .ForeignKey("dbo.StudyCourses", t => t.stdGH_studyCourseId, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.stdGH_teacherId, cascadeDelete: true)
                .Index(t => t.stdGH_schId)
                .Index(t => t.stdGH_studyCourseId)
                .Index(t => t.stdGH_teacherId)
                .Index(t => t.stdGH_examId);
            
            CreateTable(
                "dbo.StudyCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        cors_arName = c.String(nullable: false),
                        cors_enName = c.String(),
                        cors_department = c.Int(nullable: false),
                        cors_level = c.Int(nullable: false),
                        cors_term = c.Int(nullable: false),
                        cors_degree = c.Double(nullable: false),
                        cors_rate = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentGradesDets", "stdGD_studentGradesHedId", "dbo.StudentGradesHeds");
            DropForeignKey("dbo.StudentGradesHeds", "stdGH_teacherId", "dbo.Teachers");
            DropForeignKey("dbo.StudentGradesHeds", "stdGH_studyCourseId", "dbo.StudyCourses");
            DropForeignKey("dbo.StudentGradesHeds", "stdGH_schId", "dbo.Schools");
            DropForeignKey("dbo.StudentGradesHeds", "stdGH_examId", "dbo.Exams");
            DropForeignKey("dbo.StudentGradesDets", "stdGD_studentId", "dbo.Students");
            DropForeignKey("dbo.Students", "std_schId", "dbo.Schools");
            DropIndex("dbo.StudentGradesHeds", new[] { "stdGH_examId" });
            DropIndex("dbo.StudentGradesHeds", new[] { "stdGH_teacherId" });
            DropIndex("dbo.StudentGradesHeds", new[] { "stdGH_studyCourseId" });
            DropIndex("dbo.StudentGradesHeds", new[] { "stdGH_schId" });
            DropIndex("dbo.StudentGradesDets", new[] { "stdGD_studentId" });
            DropIndex("dbo.StudentGradesDets", new[] { "stdGD_studentGradesHedId" });
            DropIndex("dbo.Students", new[] { "std_schId" });
            DropTable("dbo.StudyCourses");
            DropTable("dbo.StudentGradesHeds");
            DropTable("dbo.StudentGradesDets");
            DropTable("dbo.Exams");
            DropTable("dbo.Students");
        }
    }
}
