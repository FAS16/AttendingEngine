namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAttendanceToModel8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LessonId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        CheckType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.LessonId)
                .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendances", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Attendances", "LessonId", "dbo.Lessons");
            DropIndex("dbo.Attendances", new[] { "StudentId" });
            DropIndex("dbo.Attendances", new[] { "LessonId" });
            DropTable("dbo.Attendances");
        }
    }
}
