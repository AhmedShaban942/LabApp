namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editmove3 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TeacherMovments");
            AddPrimaryKey("dbo.TeacherMovments", new[] { "mov_weekNumber", "mov_monthNumberr" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TeacherMovments");
            AddPrimaryKey("dbo.TeacherMovments", new[] { "Id", "mov_weekNumber", "mov_monthNumberr" });
        }
    }
}
