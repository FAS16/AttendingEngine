namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateOfBirthToTeacher : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teachers", "DateOfBirth", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teachers", "DateOfBirth");
        }
    }
}
