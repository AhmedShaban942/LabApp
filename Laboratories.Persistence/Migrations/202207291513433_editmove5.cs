namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editmove5 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TeacherMovments");
            AddPrimaryKey("dbo.TeacherMovments", new[] { "mov_weekNumber", "mov_monthNumberr", "mov_techId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TeacherMovments");
            AddPrimaryKey("dbo.TeacherMovments", new[] { "mov_weekNumber", "mov_monthNumberr" });
        }
    }
}
