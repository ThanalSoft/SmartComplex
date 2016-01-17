using System.Collections.Generic;

namespace ThanalSoft.SmartComplex.Web.Common.MainMenu
{
    public class SecuredMenuInfo
    {
        public bool IsMainMenu { get; set; }
        public string Text { get; set; }
        public string IconCssClass { get; set; }
        public string CssClass { get; set; }
        public string[] Roles { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }

        public MenuType MenuType { get; set; }

        public List<SecuredMenuInfo> SubMenus { get; set; }
    }

    public enum MenuType
    {
        None,
        New,
        CommingSoon
    }
}