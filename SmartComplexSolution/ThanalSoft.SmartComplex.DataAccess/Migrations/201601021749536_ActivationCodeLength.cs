namespace ThanalSoft.SmartComplex.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivationCodeLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("secure.tblUser", "ActivationCode", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("secure.tblUser", "ActivationCode", c => c.String(maxLength: 10));
        }
    }
}
