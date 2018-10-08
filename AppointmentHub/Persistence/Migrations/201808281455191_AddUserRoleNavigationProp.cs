namespace AppointmentHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserRoleNavigationProp : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
        }
    }
}
