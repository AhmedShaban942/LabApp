namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editmove4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TeacherMovments", "mov_note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeacherMovments", "mov_note");
        }
    }
}
