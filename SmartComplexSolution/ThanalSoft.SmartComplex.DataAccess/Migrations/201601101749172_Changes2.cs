namespace ThanalSoft.SmartComplex.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("sc.tblFlat", "Block", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("sc.tblFlat", "Block", c => c.String(nullable: false, maxLength: 10));
        }
    }
}
