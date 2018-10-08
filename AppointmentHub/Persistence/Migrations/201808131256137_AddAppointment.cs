namespace AppointmentHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppointment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        TimeSpan = c.Time(nullable: false, precision: 7),
                        Subject = c.String(),
                        TypeId = c.Int(nullable: false),
                        RequestedId = c.String(maxLength: 128),
                        RequesteeId = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .ForeignKey("dbo.AspNetUsers", t => t.RequestedId)
                .ForeignKey("dbo.AspNetUsers", t => t.RequesteeId)
                .ForeignKey("dbo.AppointmentTypes", t => t.TypeId, cascadeDelete: true)
                .Index(t => t.TypeId)
                .Index(t => t.RequestedId)
                .Index(t => t.RequesteeId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id1);
            
            CreateTable(
                "dbo.AppointmentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "TypeId", "dbo.AppointmentTypes");
            DropForeignKey("dbo.Appointments", "RequesteeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "RequestedId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Appointments", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Appointments", new[] { "RequesteeId" });
            DropIndex("dbo.Appointments", new[] { "RequestedId" });
            DropIndex("dbo.Appointments", new[] { "TypeId" });
            DropColumn("dbo.AspNetUsers", "Name");
            DropTable("dbo.AppointmentTypes");
            DropTable("dbo.Appointments");
        }
    }
}
