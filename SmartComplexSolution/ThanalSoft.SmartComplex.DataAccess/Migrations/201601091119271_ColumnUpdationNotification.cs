namespace ThanalSoft.SmartComplex.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnUpdationNotification : DbMigration
    {
        public override void Up()
        {
            AlterColumn("sc.tblNotification", "UserReadDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("sc.tblNotification", "UserReadDate", c => c.DateTime(nullable: false));
        }
    }
}
