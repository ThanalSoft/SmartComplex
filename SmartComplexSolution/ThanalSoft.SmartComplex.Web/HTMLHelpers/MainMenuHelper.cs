using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThanalSoft.SmartComplex.Web.Common.MainMenu;
using ThanalSoft.SmartComplex.Web.Security;

namespace ThanalSoft.SmartComplex.Web.HTMLHelpers
{
    public static class MainMenuHelper
    {
        public static MvcHtmlString SecureMainMenuFor<TModel>(this HtmlHelper<TModel> pHtmlHelper, SecuredMenuList pMenuList)
        {
            var currentUser = HttpContext.Current.User as SmartComplexPrincipal;
            if (currentUser == null)
                return MvcHtmlString.Empty;

            var menuBuilder = new StringBuilder("<div class=\"menu_section\">");
            menuBuilder.Append("<ul class=\"nav side-menu\">");
            var menus = pMenuList.Where(pX => pX.IsMainMenu && !IsMenuNotInRole<TModel>(pX, currentUser));

            foreach (var menu in menus)
            {
                menuBuilder.Append(string.IsNullOrEmpty(menu.CssClass) 
                    ? "<li>" 
                    : $"<li class=\"{menu.CssClass}\">");

                menuBuilder.Append(!string.IsNullOrEmpty(menu.Action)
                    ? $"<a href=\"{GetUrl(pHtmlHelper, menu)}\">"
                    : "<a>");

                if (!string.IsNullOrEmpty(menu.IconCssClass))
                    menuBuilder.Append($"<i class=\"fa {menu.IconCssClass}\"></i>");

                menuBuilder.Append(menu.Text);
                switch (menu.MenuType)
                {
                    case MenuType.New:
                        menuBuilder.Append("<span class=\"label label-danger pull-right\">New</span>");
                        break;
                    case MenuType.CommingSoon:
                        menuBuilder.Append("<span class=\"label label-info pull-right\">Soon!</span>");
                        break;
                }
                if (menu.SubMenus != null)
                {
                    var menusForUser = menu.SubMenus.Where(pX => !IsMenuNotInRole<TModel>(pX, currentUser));
                    if (menusForUser.Any())
                    {
                        menuBuilder.Append("<span class=\"fa fa-chevron-down\"></span>");
                        menuBuilder.Append("</a>");

                        var subMenus = GenerateSubMenus(pHtmlHelper, menu.SubMenus);
                        menuBuilder.Append(subMenus);
                    }
                    else
                        menuBuilder.Append("</a>");
                }
                menuBuilder.Append("</li>");
            }
            menuBuilder.Append("</ul>");
            menuBuilder.Append("</div>");

            return MvcHtmlString.Create(menuBuilder.ToString());
        }

        private static bool IsMenuNotInRole<TModel>(SecuredMenuInfo pMenu, SmartComplexPrincipal pCurrentUser)
        {
            return pMenu.Roles != null && pCurrentUser.Roles != null && !pCurrentUser.Roles.Any(pX => pMenu.Roles.Contains(pX));
        }

        private static string GetUrl<TModel>(HtmlHelper<TModel> pHtmlHelper, SecuredMenuInfo pMenu)
        {
            var url = UrlHelper.GenerateUrl("Default", pMenu.Action, pMenu.Controller, null, pHtmlHelper.RouteCollection, pHtmlHelper.ViewContext.RequestContext, true);
            return url;
        }

        private static string GenerateSubMenus<TModel>(HtmlHelper<TModel> pHtmlHelper, IEnumerable<SecuredMenuInfo> pSubMenus)
        {
            var subMenu = new StringBuilder("<ul class=\"nav child_menu\" style=\"display:none\">");
            foreach (var menu in pSubMenus)
            {
                subMenu.Append(string.IsNullOrEmpty(menu.CssClass) ? "<li>" : $"<li class=\"{menu.CssClass}\">");
                subMenu.Append(!string.IsNullOrEmpty(menu.Action) ? $"<a href=\"{GetUrl(pHtmlHelper, menu)}\">" : "<a>");
                subMenu.Append(menu.Text);
                switch (menu.MenuType)
                {
                    case MenuType.New:
                        subMenu.Append("<span class=\"label label-danger pull-right\">New</span>");
                        break;
                    case MenuType.CommingSoon:
                        subMenu.Append("<span class=\"label label-info pull-right\">Soon!</span>");
                        break;
                }
                subMenu.Append("</a>");
                subMenu.Append("</li>");
            }
            subMenu.Append("</ul>");
            return subMenu.ToString();
        }
    }
}