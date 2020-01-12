namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppUserToStudAndTeacher : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "ApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Teachers", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Students", "ApplicationUserId");
            CreateIndex("dbo.Teachers", "ApplicationUserId");
            AddForeignKey("dbo.Students", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Teachers", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teachers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Students", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Teachers", new[] { "ApplicationUserId" });
            DropIndex("dbo.Students", new[] { "ApplicationUserId" });
            DropColumn("dbo.Teachers", "ApplicationUserId");
            DropColumn("dbo.Students", "ApplicationUserId");
        }
    }
}
