namespace ThanalSoft.SmartComplex.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIDchangesinreftables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("sc.tblAmenityCalendar", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblApartmentAmenity", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblApartment", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblAssociation", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblAssociationMember", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblBroadcast", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblBroadcastUser", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblEvent", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblEventUser", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblFlatUser", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblFlat", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblFlatBlock", "LastUpdatedBy", c => c.Long(nullable: false));
            AlterColumn("sc.tblReminder", "LastUpdatedBy", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("sc.tblReminder", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblFlatBlock", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblFlat", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblFlatUser", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblEventUser", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblEvent", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblBroadcastUser", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblBroadcast", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblAssociationMember", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblAssociation", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblApartment", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblApartmentAmenity", "LastUpdatedBy", c => c.Int(nullable: false));
            AlterColumn("sc.tblAmenityCalendar", "LastUpdatedBy", c => c.Int(nullable: false));
        }
    }
}
