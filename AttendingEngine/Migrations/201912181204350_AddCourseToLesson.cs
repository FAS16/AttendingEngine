namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourseToLesson : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CourseClasses", newName: "ClassCourses");
            DropPrimaryKey("dbo.ClassCourses");
            AddPrimaryKey("dbo.ClassCourses", new[] { "Class_Id", "Course_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ClassCourses");
            AddPrimaryKey("dbo.ClassCourses", new[] { "Course_Id", "Class_Id" });
            RenameTable(name: "dbo.ClassCourses", newName: "CourseClasses");
        }
    }
}
