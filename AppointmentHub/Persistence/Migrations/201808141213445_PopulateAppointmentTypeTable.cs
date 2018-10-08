namespace AppointmentHub.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PopulateAppointmentTypeTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO AppointmentTypes (Name) VALUES ('Check Up')");
            Sql("INSERT INTO AppointmentTypes (Name) VALUES ('Consultation')");
            Sql("INSERT INTO AppointmentTypes (Name) VALUES ('Procedure')");
            Sql("INSERT INTO AppointmentTypes (Name) VALUES ('Out Call')");
        }

        public override void Down()
        {
            Sql("DELETE FROM AppointmentTypes WHERE Name IN ('Check Up', 'Consultation', 'Procedure', 'Out Call')");
        }
    }
}
