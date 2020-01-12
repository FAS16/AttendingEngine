namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRoomId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Rooms", "RoomIdentifier");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rooms", "RoomIdentifier", c => c.String());
        }
    }
}
