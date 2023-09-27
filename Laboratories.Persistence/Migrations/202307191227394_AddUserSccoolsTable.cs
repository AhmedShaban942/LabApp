namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserSccoolsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "usr_schId", "dbo.Schools");
            DropIndex("dbo.Users", new[] { "usr_schId" });
            CreateTable(
                "dbo.UserSchools",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.Int(),
                        School_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Schools", t => t.School_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id)
                .Index(t => t.School_Id);
            
            DropColumn("dbo.Users", "usr_schId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "usr_schId", c => c.Int());
            DropForeignKey("dbo.UserSchools", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserSchools", "School_Id", "dbo.Schools");
            DropIndex("dbo.UserSchools", new[] { "School_Id" });
            DropIndex("dbo.UserSchools", new[] { "User_Id" });
            DropTable("dbo.UserSchools");
            CreateIndex("dbo.Users", "usr_schId");
            AddForeignKey("dbo.Users", "usr_schId", "dbo.Schools", "Id");
        }
    }
}
