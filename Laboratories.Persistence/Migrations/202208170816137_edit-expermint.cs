namespace Laboratories.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editexpermint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Experiments", "expr_ExecutionDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Experiments", "expr_ExecutionDate");
        }
    }
}
