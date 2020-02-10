namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Checking : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Attendances", "DeviceId", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.Attendances", "Timestamp", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Attendances", new[] { "Timestamp" });
            AlterColumn("dbo.Attendances", "DeviceId", c => c.String(nullable: false));
        }
    }
}
