namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPossibleFraudBoolToAttendance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attendances", "PossibleFraud", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attendances", "PossibleFraud");
        }
    }
}
