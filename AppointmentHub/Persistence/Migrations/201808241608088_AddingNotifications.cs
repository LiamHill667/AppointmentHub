namespace AppointmentHub.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddingNotifications : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Appointments", new[] { "RequestedId" });
            DropIndex("dbo.Appointments", new[] { "RequesteeId" });
            DropForeignKey("dbo.Appointments", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Appointments", new[] { "ApplicationUser_Id1" });
            DropColumn("dbo.Appointments", "ApplicationUser_Id");
            DropColumn("dbo.Appointments", "ApplicationUser_Id1");
            CreateTable(
                "dbo.UserNotifications",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    NotificationId = c.Int(nullable: false),
                    IsRead = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => new { t.UserId, t.NotificationId })
                .ForeignKey("dbo.Notifications", t => t.NotificationId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.NotificationId);

            CreateTable(
                "dbo.Notifications",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DateTime = c.DateTime(nullable: false),
                    Type = c.Int(nullable: false),
                    OriginalDateTime = c.DateTime(),
                    Appointment_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Appointments", t => t.Appointment_Id, cascadeDelete: true)
                .Index(t => t.Appointment_Id);

            AlterColumn("dbo.Appointments", "Subject", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Appointments", "RequestedId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Appointments", "RequesteeId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Appointments", "RequestedId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Appointments", "RequesteeId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Appointments", "RequestedId");
            CreateIndex("dbo.Appointments", "RequesteeId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.UserNotifications", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserNotifications", "NotificationId", "dbo.Notifications");
            DropForeignKey("dbo.Notifications", "Appointment_Id", "dbo.Appointments");
            DropIndex("dbo.Notifications", new[] { "Appointment_Id" });
            DropIndex("dbo.UserNotifications", new[] { "NotificationId" });
            DropIndex("dbo.UserNotifications", new[] { "UserId" });
            DropIndex("dbo.Appointments", new[] { "RequesteeId" });
            DropIndex("dbo.Appointments", new[] { "RequestedId" });
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String());
            AlterColumn("dbo.Appointments", "RequesteeId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Appointments", "RequestedId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Appointments", "RequesteeId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Appointments", "RequestedId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Appointments", "Subject", c => c.String());
            DropTable("dbo.Notifications");
            DropTable("dbo.UserNotifications");
            AddColumn("dbo.Appointments", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.Appointments", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Appointments", "ApplicationUser_Id1");
            CreateIndex("dbo.Appointments", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "Id", "dbo.Appointments", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "Id", "dbo.Appointments", "ApplicationUser_Id1");
        }
    }
}
