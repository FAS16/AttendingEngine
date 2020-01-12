namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeviceIdToAttendance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendances", "DeviceId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attendances", "DeviceId");
        }
    }
}
