namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUpdatedRoomId3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rooms", "RoomIdentifier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rooms", "RoomIdentifier");
        }
    }
}
