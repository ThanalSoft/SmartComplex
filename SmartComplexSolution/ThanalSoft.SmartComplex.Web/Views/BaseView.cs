using System.Web.Mvc;
using ThanalSoft.SmartComplex.Web.Security;

namespace ThanalSoft.SmartComplex.Web.Views
{
    public abstract class BaseView : WebViewPage
    {
        public virtual new SmartComplexPrincipal User => base.User as SmartComplexPrincipal;

        public string Title { get; set; }
    }

    public abstract class BaseView<TModel> : WebViewPage<TModel>
    {
        public virtual new SmartComplexPrincipal User => base.User as SmartComplexPrincipal;
        public string Title { get; set; }

    }
}