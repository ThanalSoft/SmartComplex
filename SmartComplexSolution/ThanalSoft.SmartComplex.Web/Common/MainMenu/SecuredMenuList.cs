using System.Collections;
using System.Collections.Generic;

namespace ThanalSoft.SmartComplex.Web.Common.MainMenu
{
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
                    MenuType = MenuType.CommingSoon
                },
                new SecuredMenuInfo
                {
                    IsMainMenu = true,
                    IconCssClass = "fa-building",
                    Text = "Apartment",
                    Roles = new []{ "Administrator" },
                    SubMenus = new List<SecuredMenuInfo>
                    {
                        new SecuredMenuInfo
                        {
                            Roles = new []{ "Administrator" },
                            Text = "View All",
                            Action = "Index",
                            Controller = "Apartment",
                            MenuType = MenuType.New
                        }
                    }
                }
            };

            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}