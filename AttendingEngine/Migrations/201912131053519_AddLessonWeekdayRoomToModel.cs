namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLessonWeekdayRoomToModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        ClassRoom_Id = c.Int(),
                        Weekday_Id = c.Int(),
                        Course_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rooms", t => t.ClassRoom_Id)
                .ForeignKey("dbo.Weekdays", t => t.Weekday_Id)
                .ForeignKey("dbo.Courses", t => t.Course_Id)
                .Index(t => t.ClassRoom_Id)
                .Index(t => t.Weekday_Id)
                .Index(t => t.Course_Id);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoomIdentifier = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Weekdays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.String(),
                        Value = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lessons", "Course_Id", "dbo.Courses");
            DropForeignKey("dbo.Lessons", "Weekday_Id", "dbo.Weekdays");
            DropForeignKey("dbo.Lessons", "ClassRoom_Id", "dbo.Rooms");
            DropIndex("dbo.Lessons", new[] { "Course_Id" });
            DropIndex("dbo.Lessons", new[] { "Weekday_Id" });
            DropIndex("dbo.Lessons", new[] { "ClassRoom_Id" });
            DropTable("dbo.Weekdays");
            DropTable("dbo.Rooms");
            DropTable("dbo.Lessons");
        }
    }
}
