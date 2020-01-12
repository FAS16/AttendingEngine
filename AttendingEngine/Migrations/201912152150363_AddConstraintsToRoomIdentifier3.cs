namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConstraintsToRoomIdentifier3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Rooms", "RoomIdentifier", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Rooms", "RoomIdentifier", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Rooms", new[] { "RoomIdentifier" });
            AlterColumn("dbo.Rooms", "RoomIdentifier", c => c.String());
        }
    }
}
