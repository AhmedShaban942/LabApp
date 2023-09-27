namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppSetings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        companyNumber = c.Int(nullable: false),
                        compelexNumber = c.Int(nullable: false),
                        schoolNumber = c.Int(nullable: false),
                        userNumber = c.Int(nullable: false),
                        AddedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        AddedBy = c.Int(),
                        ModifiedBy = c.Int(),
                        IsAvtive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        com_arName = c.String(nullable: false),
                        com_enName = c.String(),
                        com_phon = c.String(),
                        com_email = c.String(),
                        com_address = c.String(),
                        com_image = c.Binary(),
                        AddedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        AddedBy = c.Int(),
                        ModifiedBy = c.Int(),
                        IsAvtive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Complexes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        comp_arName = c.String(nullable: false),
                        comp_enName = c.String(),
                        comp_phon = c.String(),
                        comp_email = c.String(),
                        comp_address = c.String(),
                        comp_image = c.Binary(),
                        comp_com_id = c.Int(nullable: false),
                        AddedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        AddedBy = c.Int(),
                        ModifiedBy = c.Int(),
                        IsAvtive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.comp_com_id, cascadeDelete: true)
                .Index(t => t.comp_com_id);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        sch_arName = c.String(),
                        sch_enName = c.String(),
                        sch_phon = c.String(),
                        sch_email = c.String(),
                        sch_address = c.String(),
                        sch_image = c.Binary(),
                        sch_type = c.Int(nullable: false),
                        sch_department = c.Int(nullable: false),
                        sch_comp_id = c.Int(nullable: false),
                        sch_laboratoryRecordName = c.String(),
                        AddedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        AddedBy = c.Int(),
                        ModifiedBy = c.Int(),
                        IsAvtive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Complexes", t => t.sch_comp_id, cascadeDelete: true)
                .Index(t => t.sch_comp_id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        itm_code = c.String(nullable: false),
                        itm_ministerialNo = c.Int(),
                        itm_arName = c.String(nullable: false),
                        itm_enName = c.String(),
                        itm_desc = c.String(),
                        itm_department = c.Int(nullable: false),
                        itm_level = c.Int(nullable: false),
                        itm_type = c.Int(nullable: false),
                        itm_image = c.Binary(),
                        itm_unitId = c.Int(),
                        itm_sugQty = c.Double(),
                        itm_presentQty = c.Double(),
                        itm_isExisting = c.Int(nullable: false),
                        itm_availableMethod = c.Int(nullable: false),
                        itm_chapter = c.Int(),
                        itm_term = c.Int(nullable: false),
                        itm_schId = c.Int(nullable: false),
                        itm_ValidState = c.Int(nullable: false),
                        itm_completionYear = c.Int(),
                        itm_excessiveQty = c.Double(),
                        itm_note = c.String(),
                        itm_over = c.Int(nullable: false),
                        itm_validQty = c.Double(),
                        itm_unValidQty = c.Double(),
                        itm_overQty = c.Double(),
                        AddedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        AddedBy = c.Int(),
                        ModifiedBy = c.Int(),
                        IsAvtive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.itm_schId, cascadeDelete: true)
                .ForeignKey("dbo.Units", t => t.itm_unitId)
                .Index(t => t.itm_unitId)
                .Index(t => t.itm_schId);
            
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        unt_arName = c.String(),
                        unt_enName = c.String(),
                        AddedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        AddedBy = c.Int(),
                        ModifiedBy = c.Int(),
                        IsAvtive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        usr_num = c.Int(nullable: false),
                        usr_arName = c.String(),
                        usr_enName = c.String(),
                        usr_role = c.Int(nullable: false),
                        usr_pass = c.String(),
                        usr_mobile = c.String(),
                        usr_emial = c.String(),
                        usr_address = c.String(),
                        usr_image = c.Binary(),
                        usr_schId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.usr_schId)
                .Index(t => t.usr_schId);
            
            CreateTable(
                "dbo.ScheduleDets",
                c => new
                    {
                        schd_Id = c.Int(nullable: false),
                        schd_day = c.Int(nullable: false),
                        schd_lessonNum1 = c.Int(nullable: false),
                        schd_subject1 = c.Int(nullable: false),
                        schd_lessonNum2 = c.Int(nullable: false),
                        schd_subject2 = c.Int(nullable: false),
                        schd_lessonNum3 = c.Int(nullable: false),
                        schd_subject3 = c.Int(nullable: false),
                        ScheduleDet_schd_Id = c.Int(),
                        ScheduleDet_schd_day = c.Int(),
                    })
                .PrimaryKey(t => new { t.schd_Id, t.schd_day })
                .ForeignKey("dbo.ScheduleDets", t => new { t.ScheduleDet_schd_Id, t.ScheduleDet_schd_day })
                .ForeignKey("dbo.ScheduleHeds", t => t.schd_Id, cascadeDelete: true)
                .Index(t => t.schd_Id)
                .Index(t => new { t.ScheduleDet_schd_Id, t.ScheduleDet_schd_day });
            
            CreateTable(
                "dbo.ScheduleHeds",
                c => new
                    {
                        schd_Id = c.Int(nullable: false, identity: true),
                        schd_lessonCount = c.Int(),
                        schd_teachId = c.Int(nullable: false),
                        schd_chapter = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.schd_Id)
                .ForeignKey("dbo.Teachers", t => t.schd_teachId, cascadeDelete: true)
                .Index(t => t.schd_teachId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        tech_arName = c.String(nullable: false),
                        tech_enName = c.String(),
                        tech_phon = c.String(),
                        tech_email = c.String(),
                        tech_address = c.String(),
                        tech_schId = c.Int(),
                        AddedDate = c.DateTime(),
                        ModifiedDate = c.DateTime(),
                        AddedBy = c.Int(),
                        ModifiedBy = c.Int(),
                        IsAvtive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.tech_schId)
                .Index(t => t.tech_schId);
            
            CreateTable(
                "dbo.TeacherMovments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mov_lessonCount = c.Int(),
                        mov_lessonCountActual = c.Int(),
                        mov_weekNumber = c.Int(nullable: false),
                        mov_monthNumberr = c.Int(nullable: false),
                        mov_techId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.mov_techId, cascadeDelete: true)
                .Index(t => t.mov_techId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScheduleDets", "schd_Id", "dbo.ScheduleHeds");
            DropForeignKey("dbo.ScheduleHeds", "schd_teachId", "dbo.Teachers");
            DropForeignKey("dbo.Teachers", "tech_schId", "dbo.Schools");
            DropForeignKey("dbo.TeacherMovments", "mov_techId", "dbo.Teachers");
            DropForeignKey("dbo.ScheduleDets", new[] { "ScheduleDet_schd_Id", "ScheduleDet_schd_day" }, "dbo.ScheduleDets");
            DropForeignKey("dbo.Users", "usr_schId", "dbo.Schools");
            DropForeignKey("dbo.Items", "itm_unitId", "dbo.Units");
            DropForeignKey("dbo.Items", "itm_schId", "dbo.Schools");
            DropForeignKey("dbo.Schools", "sch_comp_id", "dbo.Complexes");
            DropForeignKey("dbo.Complexes", "comp_com_id", "dbo.Companies");
            DropIndex("dbo.TeacherMovments", new[] { "mov_techId" });
            DropIndex("dbo.Teachers", new[] { "tech_schId" });
            DropIndex("dbo.ScheduleHeds", new[] { "schd_teachId" });
            DropIndex("dbo.ScheduleDets", new[] { "ScheduleDet_schd_Id", "ScheduleDet_schd_day" });
            DropIndex("dbo.ScheduleDets", new[] { "schd_Id" });
            DropIndex("dbo.Users", new[] { "usr_schId" });
            DropIndex("dbo.Items", new[] { "itm_schId" });
            DropIndex("dbo.Items", new[] { "itm_unitId" });
            DropIndex("dbo.Schools", new[] { "sch_comp_id" });
            DropIndex("dbo.Complexes", new[] { "comp_com_id" });
            DropTable("dbo.TeacherMovments");
            DropTable("dbo.Teachers");
            DropTable("dbo.ScheduleHeds");
            DropTable("dbo.ScheduleDets");
            DropTable("dbo.Users");
            DropTable("dbo.Units");
            DropTable("dbo.Items");
            DropTable("dbo.Schools");
            DropTable("dbo.Complexes");
            DropTable("dbo.Companies");
            DropTable("dbo.AppSetings");
        }
    }
}
