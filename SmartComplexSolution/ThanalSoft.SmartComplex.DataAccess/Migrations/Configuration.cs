using System;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ThanalSoft.SmartComplex.Entities.Common;
using ThanalSoft.SmartComplex.Entities.Complex;
using ThanalSoft.SmartComplex.Entities.Security;

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

            CreateFlatTypes(pContext);

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
            var userManager = new UserManager<LoginUser, long>(new UserStore<LoginUser, Role, Int64, UserLogin, UserRole, UserClaim>(new SmartComplexDataObjectContext()));

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
                user = new LoginUser
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
                    UserName = "admin@sc.com",
                    IsDeleted = false,
                    FirstName = "Administrator",
                    IsFreezed = false
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

        private void CreateFlatTypes(SmartComplexDataObjectContext pContext)
        {
            pContext.FlatTypes.AddOrUpdate(pName => pName.Name,
                new FlatType
                {
                    Description = "1 Bed Room, Hall & Kitchen",
                    IsActive = true,
                    Name = "1 BHK"
                },
                new FlatType
                {
                    Description = "1 Bed Room, Hall & Kitchen",
                    IsActive = true,
                    Name = "1.5 BHK"
                },
                new FlatType
                {
                    Description = "2 Bed Room, Hall & Kitchen",
                    IsActive = true,
                    Name = "2 BHK"
                },
                new FlatType
                {
                    Description = "2.5 Bed Room, Hall & Kitchen",
                    IsActive = true,
                    Name = "2.5 BHK"
                },
                new FlatType
                {
                    Description = "3 Bed Room, Hall & Kitchen",
                    IsActive = true,
                    Name = "3 BHK"
                },
                new FlatType
                {
                    Description = "3.5 Bed Room, Hall & Kitchen",
                    IsActive = true,
                    Name = "3.5 BHK"
                },
                new FlatType
                {
                    Description = "4 Bed Room, Hall & Kitchen",
                    IsActive = true,
                    Name = "4 BHK"
                },
                new FlatType
                {
                    Description = "4.5 Bed Room, Hall & Kitchen",
                    IsActive = true,
                    Name = "4.5 BHK"
                },
                new FlatType
                {
                    Description = "5 Bed Room, Hall & Kitchen",
                    IsActive = true,
                    Name = "5 BHK"
                });
            pContext.SaveChanges();
        }

    }
}