namespace ThanalSoft.SmartComplex.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "sc.tblFlatType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 200),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("sc.tblFlat", "FlatTypeId", c => c.Int());
            CreateIndex("sc.tblFlat", "FlatTypeId");
            AddForeignKey("sc.tblFlat", "FlatTypeId", "sc.tblFlatType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("sc.tblFlat", "FlatTypeId", "sc.tblFlatType");
            DropIndex("sc.tblFlat", new[] { "FlatTypeId" });
            DropColumn("sc.tblFlat", "FlatTypeId");
            DropTable("sc.tblFlatType");
        }
    }
}
