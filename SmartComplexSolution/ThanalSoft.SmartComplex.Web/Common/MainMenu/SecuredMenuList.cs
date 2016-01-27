using System.Collections;
using System.Collections.Generic;

namespace ThanalSoft.SmartComplex.Web.Common.MainMenu
{
//      Administrator
//      ApartmentAdmin
//      MaintenanceManager
//      Owner
//      Tenant
    public class SecuredMenuList : IEnumerable<SecuredMenuInfo>
    {
        public IEnumerator<SecuredMenuInfo> GetEnumerator()
        {
            var list = new List<SecuredMenuInfo>
            {
                new SecuredMenuInfo
                {
                    IconCssClass = "fa-tachometer",
                    CssClass = "nv active",
                    IsMainMenu = true,
                    Text = "Dashboard",
                    Action = "Index",
                    Controller = "Home",
                    Area = "Dashboard",
                    MenuType = MenuType.CommingSoon
                },
                new SecuredMenuInfo
                {
                    IsMainMenu = true,
                    IconCssClass = "fa-building-o",
                    Text = "Apartment",
                    Roles = new []{ "ApartmentAdmin", "Administrator", "Tenant", "Owner", "MaintenanceManager" },
                    SubMenus = new List<SecuredMenuInfo>
                    {
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "Administrator" },
                            Text = "View All",
                            Action = "GetAll",
                            Controller = "Manage",
                            Area = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin", "Tenant", "Owner", "MaintenanceManager"  },
                            Text = "Apartment",
                            Action = "Index",
                            Controller = "Home",
                            Area = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "Tenant", "Owner" },
                            Text = "Flat",
                            Action = "Index",
                            Controller = "Apartment",
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin", "Tenant", "Owner" },
                            Text = "Association",
                            Action = "Index",
                            Controller = "Apartment",
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Assets",
                            Action = "Index",
                            Controller = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Amenities",
                            Action = "Index",
                            Controller = "Apartment",
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Settings",
                            Action = "Index",
                            Controller = "Apartment",
                        }
                    }
                },
                new SecuredMenuInfo
                {
                    IsMainMenu = true,
                    IconCssClass = "fa-inr",
                    Text = "Accounts",
                    Roles = new []{ "ApartmentAdmin" },
                    SubMenus = new List<SecuredMenuInfo>
                    {
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Income",
                            Action = "Index",
                            Controller = "Apartment",
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Expense",
                            Action = "Index",
                            Controller = "Apartment",
                        }
                    }
                },
                new SecuredMenuInfo
                {
                    IsMainMenu = true,
                    IconCssClass = "fa-users",
                    Text = "Members",
                    Roles = new []{ "ApartmentAdmin" },
                    SubMenus = new List<SecuredMenuInfo>
                    {
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Residence",
                            Action = "Index",
                            Controller = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Owners",
                            Action = "Index",
                            Controller = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Tenants",
                            Action = "Index",
                            Controller = "Apartment"
                        }
                    }
                },
                new SecuredMenuInfo
                {
                    IsMainMenu = true,
                    IconCssClass = "fa-folder-open",
                    Text = "Documents",
                    Roles = new []{ "ApartmentAdmin" },
                    SubMenus = new List<SecuredMenuInfo>
                    {
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Management",
                            Action = "Index",
                            Controller = "Apartment"
                        }
                    }
                },
                new SecuredMenuInfo
                {
                    IsMainMenu = true,
                    IconCssClass = "fa-bullhorn",
                    Text = "Communication",
                    Roles = new []{ "ApartmentAdmin" },
                    SubMenus = new List<SecuredMenuInfo>
                    {
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Reminders",
                            Action = "Index",
                            Controller = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Meetings",
                            Action = "Index",
                            Controller = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Discussions",
                            Action = "Index",
                            Controller = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Messages",
                            Action = "Index",
                            Controller = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Voting",
                            Action = "Index",
                            Controller = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Complaints/Sugessions",
                            Action = "Index",
                            Controller = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Events",
                            Action = "Index",
                            Controller = "Apartment"
                        }
                    }
                },
                new SecuredMenuInfo
                {
                    IsMainMenu = true,
                    IconCssClass = "fa-bug",
                    Text = "Maintenance",
                    Roles = new []{ "ApartmentAdmin" },
                    SubMenus = new List<SecuredMenuInfo>
                    {
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Request",
                            Action = "Index",
                            Controller = "Apartment"
                        }
                    }
                },
                new SecuredMenuInfo
                {
                    IsMainMenu = true,
                    IconCssClass = "fa-meh-o",
                    Text = "Employees",
                    Roles = new []{ "ApartmentAdmin" },
                    SubMenus = new List<SecuredMenuInfo>
                    {
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Management",
                            Action = "Index",
                            Controller = "Apartment"
                        }
                    }
                },
                new SecuredMenuInfo
                {
                    IsMainMenu = true,
                    IconCssClass = "fa-phone-square",
                    Text = "Contacts",
                    Roles = new []{ "ApartmentAdmin" },
                    SubMenus = new List<SecuredMenuInfo>
                    {
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Employee Contacts",
                            Action = "Index",
                            Controller = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Important Contacts",
                            Action = "Index",
                            Controller = "Apartment"
                        },
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "ApartmentAdmin" },
                            Text = "Shared Contacts",
                            Action = "Index",
                            Controller = "Apartment"
                        }
                    }
                },
            };

            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}