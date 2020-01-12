namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClassAndCourseToModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Teacher_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teachers", t => t.Teacher_Id)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.CourseClasses",
                c => new
                    {
                        Course_Id = c.Int(nullable: false),
                        Class_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Course_Id, t.Class_Id })
                .ForeignKey("dbo.Courses", t => t.Course_Id, cascadeDelete: true)
                .ForeignKey("dbo.Classes", t => t.Class_Id, cascadeDelete: true)
                .Index(t => t.Course_Id)
                .Index(t => t.Class_Id);
            
            AddColumn("dbo.Students", "Class_Id", c => c.Int());
            CreateIndex("dbo.Students", "Class_Id");
            AddForeignKey("dbo.Students", "Class_Id", "dbo.Classes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.Courses", "Teacher_Id", "dbo.Teachers");
            DropForeignKey("dbo.CourseClasses", "Class_Id", "dbo.Classes");
            DropForeignKey("dbo.CourseClasses", "Course_Id", "dbo.Courses");
            DropIndex("dbo.CourseClasses", new[] { "Class_Id" });
            DropIndex("dbo.CourseClasses", new[] { "Course_Id" });
            DropIndex("dbo.Courses", new[] { "Teacher_Id" });
            DropIndex("dbo.Students", new[] { "Class_Id" });
            DropColumn("dbo.Students", "Class_Id");
            DropTable("dbo.CourseClasses");
            DropTable("dbo.Courses");
            DropTable("dbo.Classes");
        }
    }
}
