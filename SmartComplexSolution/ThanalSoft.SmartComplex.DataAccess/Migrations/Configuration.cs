using System;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ThanalSoft.SmartComplex.DataObjects.Common;
using ThanalSoft.SmartComplex.DataObjects.Complex;
using ThanalSoft.SmartComplex.DataObjects.Security;

namespace ThanalSoft.SmartComplex.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SmartComplexDataObjectContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(SmartComplexDataObjectContext pContext)
        {
            CreateBloodGroup(pContext);

            CreateCountriesAndStates(pContext);

            CreateAdministratorUserAndLogin(pContext);

            CreateAminityTypes(pContext);

            CreateAssosiationMemberRanks(pContext);

            pContext.SaveChanges();
        }

        private void CreateAssosiationMemberRanks(SmartComplexDataObjectContext pContext)
        {
            pContext.AssociationMemberRanks.AddOrUpdate(pName => pName.Name,
                new AssociationMemberRank
                {
                    Name = "President",
                },
                new AssociationMemberRank
                {
                    Name = "Vice President"
                },
                new AssociationMemberRank
                {
                    Name = "Secretary"
                },
                new AssociationMemberRank
                {
                    Name = "Joint Secretary"
                },
                new AssociationMemberRank
                {
                    Name = "Treasurer"
                },
                new AssociationMemberRank
                {
                    Name = "Executive Member"
                },
                new AssociationMemberRank
                {
                    Name = "Non Executive Member"
                }
            );

            pContext.SaveChanges();
        }

        private void CreateAminityTypes(SmartComplexDataObjectContext pContext)
        {
            pContext.AminityTypes.AddOrUpdate(pName => pName.Name,
                new AmenityType
                {
                    IsActive = true,
                    Name = "Clubhouse"
                },
                new AmenityType
                {
                    IsActive = true,
                    Name = "Gym"
                },
                new AmenityType
                {
                    IsActive = true,
                    Name = "Swimming Pool"
                },
                new AmenityType
                {
                    IsActive = true,
                    Name = "Children Play Area"
                },
                new AmenityType
                {
                    IsActive = true,
                    Name = "Batminton Court"
                },
                new AmenityType
                {
                    IsActive = true,
                    Name = "Cricket Ground"
                },
                new AmenityType
                {
                    IsActive = true,
                    Name = "Football Ground"
                },
                new AmenityType
                {
                    IsActive = true,
                    Name = "Baskeball Ground"
                }
            );

            pContext.SaveChanges();
        }

        private void CreateAdministratorUserAndLogin(SmartComplexDataObjectContext pContext)
        {
            var roleManager = new RoleManager<Role, long>(new RoleStore<Role, Int64, UserRole>(new SmartComplexDataObjectContext()));
            var userManager = new UserManager<User, long>(new UserStore<User, Role, Int64, UserLogin, UserRole, UserClaim>(new SmartComplexDataObjectContext()));

            var role = roleManager.FindByName("Administrator");
            if (role == null)
            {
                role = new Role("Administrator");
                roleManager.Create(role);
            }
            role = roleManager.FindByName("Owner");
            if (role == null)
            {
                role = new Role("Owner");
                roleManager.Create(role);
            }
            role = roleManager.FindByName("Tenant");
            if (role == null)
            {
                role = new Role("Tenant");
                roleManager.Create(role);
            }
            role = roleManager.FindByName("MaintenanceAdmin");
            if (role == null)
            {
                role = new Role("MaintenanceManager");
                roleManager.Create(role);
            }
            role = roleManager.FindByName("ApartmentAdmin");
            if (role == null)
            {
                role = new Role("ApartmentAdmin");
                roleManager.Create(role);
            }
            var user = userManager.FindByEmail("admin@sc.com");
            if (user == null)
            {
                var hasher = new PasswordHasher();
                user = new User
                {
                    Email = "admin@sc.com",
                    PhoneNumber = "9742170983",
                    AccessFailedCount = 0,
                    ActivatedDate = null,
                    ActivationCode = "ADMIN",
                    EmailConfirmed = true,
                    IsActivated = true,
                    LockoutEnabled = false,
                    LockoutEndDateUtc = null,
                    PasswordHash = hasher.HashPassword("admin"),
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    IsAdminUser = true,
                    UserName = "admin@sc.com"
                };

                userManager.Create(user);
            }
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains("Administrator"))
            {
                userManager.AddToRole(user.Id, "Administrator");
            }
            pContext.SaveChanges();
        }

        private void CreateCountriesAndStates(SmartComplexDataObjectContext pContext)
        {
            pContext.Countries.AddOrUpdate(pName => pName.Code,
                new Country
                {
                    Code = "IN",
                    Name = "India"
                });
            pContext.SaveChanges();

            pContext.States.AddOrUpdate(pName => pName.Name,
                new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Andhra Pradesh"
                },
                new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Arunachal Pradesh"
                },
                new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Assam"
                },
                new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Bihar"
                },
                new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Chhattisgarh"
                },
                new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Goa"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Gujarat"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Haryana"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Himachal Pradesh"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Jammu and Kashmir"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Jharkhand"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Karnataka"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Kerala"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Madhya Pradesh"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Maharashtra"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Manipur"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Meghalaya"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Mizoram"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Nagaland"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Orissa"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Punjab"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Rajasthan"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Sikkim"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Tamil Nadu"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Telangana"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Tripura"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Uttarakhand"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Uttar Pradesh"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "West Bengal"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "Chandigarh"
                }, new State
                {
                    CountryId = pContext.Countries.Single(pX => pX.Code.Equals("IN")).Id,
                    Name = "New Delhi"
                });
        }

        private void CreateBloodGroup(SmartComplexDataObjectContext pContext)
        {
            pContext.BloodGroups.AddOrUpdate(pName => pName.Group,
                new BloodGroup
                {
                    Group = "A+"
                },
                new BloodGroup
                {
                    Group = "A-"
                },
                new BloodGroup
                {
                    Group = "B+"
                },
                new BloodGroup
                {
                    Group = "B-"
                },
                new BloodGroup
                {
                    Group = "AB+"
                },
                new BloodGroup
                {
                    Group = "AB-"
                },
                new BloodGroup
                {
                    Group = "O+"
                },
                new BloodGroup
                {
                    Group = "O-"
                });
            pContext.SaveChanges();
        }
    }
}
