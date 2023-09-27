namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateUserRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        role_id = c.Int(nullable: false, identity: true),
                        role_name = c.String(),
                        role_enName = c.String(),
                    })
                .PrimaryKey(t => t.role_id);
            
            CreateTable(
                "dbo.ScreenRoles",
                c => new
                    {
                        screen_role_id = c.Int(nullable: false, identity: true),
                        role_id = c.Int(),
                        screen_id = c.Int(),
                    })
                .PrimaryKey(t => t.screen_role_id)
                .ForeignKey("dbo.UserRoles", t => t.role_id)
                .ForeignKey("dbo.Screens", t => t.screen_id)
                .Index(t => t.role_id)
                .Index(t => t.screen_id);
            
            CreateTable(
                "dbo.Screens",
                c => new
                    {
                        screen_id = c.Int(nullable: false),
                        screen_name = c.String(),
                        screen_enName = c.String(),
                    })
                .PrimaryKey(t => t.screen_id);
            
            AddColumn("dbo.Users", "usr_roleId", c => c.Int());
            CreateIndex("dbo.Users", "usr_roleId");
            AddForeignKey("dbo.Users", "usr_roleId", "dbo.UserRoles", "role_id");
            DropColumn("dbo.Users", "usr_role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "usr_role", c => c.Int(nullable: false));
            DropForeignKey("dbo.Users", "usr_roleId", "dbo.UserRoles");
            DropForeignKey("dbo.ScreenRoles", "screen_id", "dbo.Screens");
            DropForeignKey("dbo.ScreenRoles", "role_id", "dbo.UserRoles");
            DropIndex("dbo.ScreenRoles", new[] { "screen_id" });
            DropIndex("dbo.ScreenRoles", new[] { "role_id" });
            DropIndex("dbo.Users", new[] { "usr_roleId" });
            DropColumn("dbo.Users", "usr_roleId");
            DropTable("dbo.Screens");
            DropTable("dbo.ScreenRoles");
            DropTable("dbo.UserRoles");
        }
    }
}
