namespace AttendingEngine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFahadToStudent : DbMigration
    {
        public override void Up()
        {
            Sql("SET IDENTITY_INSERT Students ON");
            Sql("INSERT INTO Students (Id, Name, Email) VALUES (1, 'Fahad Ali', 'fahad@elev.dk')");
            Sql("SET IDENTITY_INSERT Students OFF");

        }

        public override void Down()
        {
        }
    }
}
