namespace ThanalSoft.SmartComplex.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "sc.tblAmenityCalendar",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AminityTypeId = c.Int(nullable: false),
                        BookedUserId = c.Long(nullable: false),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        Reason = c.String(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("sc.tblAmenityType", t => t.AminityTypeId)
                .ForeignKey("secure.tblUser", t => t.BookedUserId)
                .Index(t => t.AminityTypeId)
                .Index(t => t.BookedUserId);
            
            CreateTable(
                "sc.tblAmenityType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "sc.tblApartmentAmenity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApartmentId = c.Int(nullable: false),
                        AminityTypeId = c.Int(nullable: false),
                        IsBillable = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("sc.tblAmenityType", t => t.AminityTypeId)
                .ForeignKey("sc.tblApartment", t => t.ApartmentId)
                .Index(t => t.ApartmentId)
                .Index(t => t.AminityTypeId);
            
            CreateTable(
                "sc.tblApartment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Address = c.String(nullable: false),
                        City = c.String(nullable: false, maxLength: 150),
                        StateId = c.Int(nullable: false),
                        PinCode = c.Int(nullable: false),
                        Phone = c.String(),
                        IsLocked = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LockedDate = c.DateTime(),
                        LockReason = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("sc.tblState", t => t.StateId)
                .Index(t => t.StateId);
            
            CreateTable(
                "sc.tblAssociation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApartmentId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 150),
                        Description = c.String(nullable: false, maxLength: 150),
                        CreationDate = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("sc.tblApartment", t => t.ApartmentId)
                .Index(t => t.ApartmentId);
            
            CreateTable(
                "sc.tblAssociationMember",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssociationId = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                        AssociationMemberRankId = c.Int(nullable: false),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("sc.tblAssociation", t => t.AssociationId)
                .ForeignKey("sc.tblAssociationMemberRank", t => t.AssociationMemberRankId)
                .ForeignKey("secure.tblUser", t => t.UserId)
                .Index(t => t.AssociationId)
                .Index(t => t.UserId)
                .Index(t => t.AssociationMemberRankId);
            
            CreateTable(
                "sc.tblAssociationMemberRank",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "secure.tblUser",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsAdminUser = c.Boolean(nullable: false),
                        ActivationCode = c.String(maxLength: 100),
                        IsActivated = c.Boolean(nullable: false),
                        ActivatedDate = c.DateTime(),
                        FirstName = c.String(nullable: false, maxLength: 250),
                        LastName = c.String(maxLength: 250),
                        BloodGroupId = c.Int(),
                        IsFreezed = c.Boolean(nullable: false),
                        FreezedDate = c.DateTime(),
                        ReasonForFreeze = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Email = c.String(nullable: false, maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(nullable: false),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("sc.tblBloodGroup", t => t.BloodGroupId)
                .Index(t => t.BloodGroupId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "sc.tblBloodGroup",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group = c.String(nullable: false, maxLength: 5),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "sc.tblBroadcast",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false),
                        CreatorId = c.Long(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        IsAlert = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("secure.tblUser", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "sc.tblBroadcastUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReceiverUserId = c.Long(nullable: false),
                        BroadcastId = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("sc.tblBroadcast", t => t.BroadcastId)
                .ForeignKey("secure.tblUser", t => t.ReceiverUserId)
                .Index(t => t.ReceiverUserId)
                .Index(t => t.BroadcastId);
            
            CreateTable(
                "secure.tblUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("secure.tblUser", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "sc.tblEvent",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        CreatorId = c.Long(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("secure.tblUser", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "sc.tblEventUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        EventUserId = c.Long(nullable: false),
                        IsEventAdmin = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("sc.tblEvent", t => t.EventId)
                .ForeignKey("secure.tblUser", t => t.EventUserId)
                .Index(t => t.EventId)
                .Index(t => t.EventUserId);
            
            CreateTable(
                "secure.tblUserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("secure.tblUser", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "sc.tblMemberFlat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApartmentId = c.Int(nullable: false),
                        FlatId = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                        IsOwner = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("sc.tblApartment", t => t.ApartmentId)
                .ForeignKey("sc.tblFlat", t => t.FlatId)
                .ForeignKey("secure.tblUser", t => t.UserId)
                .Index(t => t.ApartmentId)
                .Index(t => t.FlatId)
                .Index(t => t.UserId);
            
            CreateTable(
                "sc.tblFlat",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApartmentId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        FlatTypeId = c.Int(),
                        Floor = c.Int(nullable: false),
                        Block = c.String(maxLength: 10),
                        Phase = c.String(maxLength: 10),
                        ExtensionNumber = c.Int(),
                        SquareFeet = c.Int(),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("sc.tblApartment", t => t.ApartmentId)
                .ForeignKey("sc.tblFlatType", t => t.FlatTypeId)
                .Index(t => t.ApartmentId)
                .Index(t => t.FlatTypeId);
            
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
            
            CreateTable(
                "sc.tblNotification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        TargetUserId = c.Long(nullable: false),
                        HasUserRead = c.Boolean(nullable: false),
                        UserReadDate = c.DateTime(),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("secure.tblUser", t => t.TargetUserId)
                .Index(t => t.TargetUserId);
            
            CreateTable(
                "sc.tblReminder",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 200),
                        Description = c.String(maxLength: 300),
                        CreatorId = c.Long(nullable: false),
                        CreatedTime = c.DateTime(nullable: false),
                        ExpiryTime = c.DateTime(nullable: false),
                        ReminderCount = c.Short(nullable: false),
                        ReminderTime = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                        LastUpdatedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("secure.tblUser", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "secure.tblUserRole",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("secure.tblUser", t => t.UserId)
                .ForeignKey("secure.tblRole", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "sc.tblState",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        CountryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("sc.tblCountry", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "sc.tblCountry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 5),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "secure.tblRole",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("secure.tblUserRole", "RoleId", "secure.tblRole");
            DropForeignKey("sc.tblState", "CountryId", "sc.tblCountry");
            DropForeignKey("sc.tblApartment", "StateId", "sc.tblState");
            DropForeignKey("secure.tblUserRole", "UserId", "secure.tblUser");
            DropForeignKey("sc.tblReminder", "CreatorId", "secure.tblUser");
            DropForeignKey("sc.tblNotification", "TargetUserId", "secure.tblUser");
            DropForeignKey("sc.tblMemberFlat", "UserId", "secure.tblUser");
            DropForeignKey("sc.tblMemberFlat", "FlatId", "sc.tblFlat");
            DropForeignKey("sc.tblFlat", "FlatTypeId", "sc.tblFlatType");
            DropForeignKey("sc.tblFlat", "ApartmentId", "sc.tblApartment");
            DropForeignKey("sc.tblMemberFlat", "ApartmentId", "sc.tblApartment");
            DropForeignKey("secure.tblUserLogin", "UserId", "secure.tblUser");
            DropForeignKey("sc.tblEventUser", "EventUserId", "secure.tblUser");
            DropForeignKey("sc.tblEventUser", "EventId", "sc.tblEvent");
            DropForeignKey("sc.tblEvent", "CreatorId", "secure.tblUser");
            DropForeignKey("secure.tblUserClaim", "UserId", "secure.tblUser");
            DropForeignKey("sc.tblBroadcastUser", "ReceiverUserId", "secure.tblUser");
            DropForeignKey("sc.tblBroadcastUser", "BroadcastId", "sc.tblBroadcast");
            DropForeignKey("sc.tblBroadcast", "CreatorId", "secure.tblUser");
            DropForeignKey("secure.tblUser", "BloodGroupId", "sc.tblBloodGroup");
            DropForeignKey("sc.tblAssociationMember", "UserId", "secure.tblUser");
            DropForeignKey("sc.tblAmenityCalendar", "BookedUserId", "secure.tblUser");
            DropForeignKey("sc.tblAssociationMember", "AssociationMemberRankId", "sc.tblAssociationMemberRank");
            DropForeignKey("sc.tblAssociationMember", "AssociationId", "sc.tblAssociation");
            DropForeignKey("sc.tblAssociation", "ApartmentId", "sc.tblApartment");
            DropForeignKey("sc.tblApartmentAmenity", "ApartmentId", "sc.tblApartment");
            DropForeignKey("sc.tblApartmentAmenity", "AminityTypeId", "sc.tblAmenityType");
            DropForeignKey("sc.tblAmenityCalendar", "AminityTypeId", "sc.tblAmenityType");
            DropIndex("secure.tblRole", "RoleNameIndex");
            DropIndex("sc.tblState", new[] { "CountryId" });
            DropIndex("secure.tblUserRole", new[] { "RoleId" });
            DropIndex("secure.tblUserRole", new[] { "UserId" });
            DropIndex("sc.tblReminder", new[] { "CreatorId" });
            DropIndex("sc.tblNotification", new[] { "TargetUserId" });
            DropIndex("sc.tblFlat", new[] { "FlatTypeId" });
            DropIndex("sc.tblFlat", new[] { "ApartmentId" });
            DropIndex("sc.tblMemberFlat", new[] { "UserId" });
            DropIndex("sc.tblMemberFlat", new[] { "FlatId" });
            DropIndex("sc.tblMemberFlat", new[] { "ApartmentId" });
            DropIndex("secure.tblUserLogin", new[] { "UserId" });
            DropIndex("sc.tblEventUser", new[] { "EventUserId" });
            DropIndex("sc.tblEventUser", new[] { "EventId" });
            DropIndex("sc.tblEvent", new[] { "CreatorId" });
            DropIndex("secure.tblUserClaim", new[] { "UserId" });
            DropIndex("sc.tblBroadcastUser", new[] { "BroadcastId" });
            DropIndex("sc.tblBroadcastUser", new[] { "ReceiverUserId" });
            DropIndex("sc.tblBroadcast", new[] { "CreatorId" });
            DropIndex("secure.tblUser", "UserNameIndex");
            DropIndex("secure.tblUser", new[] { "BloodGroupId" });
            DropIndex("sc.tblAssociationMember", new[] { "AssociationMemberRankId" });
            DropIndex("sc.tblAssociationMember", new[] { "UserId" });
            DropIndex("sc.tblAssociationMember", new[] { "AssociationId" });
            DropIndex("sc.tblAssociation", new[] { "ApartmentId" });
            DropIndex("sc.tblApartment", new[] { "StateId" });
            DropIndex("sc.tblApartmentAmenity", new[] { "AminityTypeId" });
            DropIndex("sc.tblApartmentAmenity", new[] { "ApartmentId" });
            DropIndex("sc.tblAmenityCalendar", new[] { "BookedUserId" });
            DropIndex("sc.tblAmenityCalendar", new[] { "AminityTypeId" });
            DropTable("secure.tblRole");
            DropTable("sc.tblCountry");
            DropTable("sc.tblState");
            DropTable("secure.tblUserRole");
            DropTable("sc.tblReminder");
            DropTable("sc.tblNotification");
            DropTable("sc.tblFlatType");
            DropTable("sc.tblFlat");
            DropTable("sc.tblMemberFlat");
            DropTable("secure.tblUserLogin");
            DropTable("sc.tblEventUser");
            DropTable("sc.tblEvent");
            DropTable("secure.tblUserClaim");
            DropTable("sc.tblBroadcastUser");
            DropTable("sc.tblBroadcast");
            DropTable("sc.tblBloodGroup");
            DropTable("secure.tblUser");
            DropTable("sc.tblAssociationMemberRank");
            DropTable("sc.tblAssociationMember");
            DropTable("sc.tblAssociation");
            DropTable("sc.tblApartment");
            DropTable("sc.tblApartmentAmenity");
            DropTable("sc.tblAmenityType");
            DropTable("sc.tblAmenityCalendar");
        }
    }
}
