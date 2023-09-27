namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editDet : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ScheduleDets", new[] { "ScheduleDet_schd_Id", "ScheduleDet_schd_day" }, "dbo.ScheduleDets");
            DropForeignKey("dbo.ScheduleDets", "schd_Id", "dbo.ScheduleHeds");
            DropIndex("dbo.ScheduleDets", new[] { "schd_Id" });
            DropIndex("dbo.ScheduleDets", new[] { "ScheduleDet_schd_Id", "ScheduleDet_schd_day" });
            AddColumn("dbo.ScheduleDets", "schd_scheduleHedId", c => c.Int(nullable: false));
            CreateIndex("dbo.ScheduleDets", "schd_scheduleHedId");
            AddForeignKey("dbo.ScheduleDets", "schd_scheduleHedId", "dbo.ScheduleHeds", "schd_Id", cascadeDelete: true);
            DropColumn("dbo.ScheduleDets", "ScheduleDet_schd_Id");
            DropColumn("dbo.ScheduleDets", "ScheduleDet_schd_day");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ScheduleDets", "ScheduleDet_schd_day", c => c.Int());
            AddColumn("dbo.ScheduleDets", "ScheduleDet_schd_Id", c => c.Int());
            DropForeignKey("dbo.ScheduleDets", "schd_scheduleHedId", "dbo.ScheduleHeds");
            DropIndex("dbo.ScheduleDets", new[] { "schd_scheduleHedId" });
            DropColumn("dbo.ScheduleDets", "schd_scheduleHedId");
            CreateIndex("dbo.ScheduleDets", new[] { "ScheduleDet_schd_Id", "ScheduleDet_schd_day" });
            CreateIndex("dbo.ScheduleDets", "schd_Id");
            AddForeignKey("dbo.ScheduleDets", "schd_Id", "dbo.ScheduleHeds", "schd_Id", cascadeDelete: true);
            AddForeignKey("dbo.ScheduleDets", new[] { "ScheduleDet_schd_Id", "ScheduleDet_schd_day" }, "dbo.ScheduleDets", new[] { "schd_Id", "schd_day" });
        }
    }
}
