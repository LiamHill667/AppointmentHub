namespace AppointmentHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAvailability : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAvailabilities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        TimeSpan = c.Time(nullable: false, precision: 7),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAvailabilities", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserAvailabilities", new[] { "UserId" });
            DropTable("dbo.UserAvailabilities");
        }
    }
}
