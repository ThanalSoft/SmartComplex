using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ThanalSoft.SmartComplex.Entities.Security;

namespace ThanalSoft.SmartComplex.DataAccess
{
    public class SmartComplexDataObjectContext : BaseDataAccessContext
    {
        protected override void OnModelCreating(DbModelBuilder pModelBuilder)
        {
            base.OnModelCreating(pModelBuilder);

            pModelBuilder.HasDefaultSchema("sc");

            SecureTables(pModelBuilder);
        }

        private static void SecureTables(DbModelBuilder pModelBuilder)
        {
            pModelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            pModelBuilder.Entity<Role>().ToTable("tblRole", "secure");
            pModelBuilder.Entity<UserClaim>().ToTable("tblUserClaim", "secure");
            pModelBuilder.Entity<UserLogin>().ToTable("tblUserLogin", "secure");
            pModelBuilder.Entity<UserRole>().ToTable("tblUserRole", "secure");
            pModelBuilder.Entity<LoginUser>().ToTable("tblUser", "secure")
                .Property(pX => pX.Email)
                .IsRequired();
            pModelBuilder.Entity<LoginUser>()
                .Property(pX => pX.PhoneNumber)
                .IsRequired();
        }

        public static SmartComplexDataObjectContext Create()
        {
            return new SmartComplexDataObjectContext();
        }
    }
}