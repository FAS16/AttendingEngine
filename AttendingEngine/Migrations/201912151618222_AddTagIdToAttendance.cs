namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTagIdToAttendance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendances", "TagId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attendances", "TagId");
        }
    }
}
