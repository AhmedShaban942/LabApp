namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editMobv : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TeacherMovments");
            AlterColumn("dbo.TeacherMovments", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.TeacherMovments", new[] { "Id", "mov_weekNumber", "mov_monthNumberr" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TeacherMovments");
            AlterColumn("dbo.TeacherMovments", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.TeacherMovments", "Id");
        }
    }
}
