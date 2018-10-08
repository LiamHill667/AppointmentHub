namespace AppointmentHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnIsCanceledAppointment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "IsCanceled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "IsCanceled");
        }
    }
}
