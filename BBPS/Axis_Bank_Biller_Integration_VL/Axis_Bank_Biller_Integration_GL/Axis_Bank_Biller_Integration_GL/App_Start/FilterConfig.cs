using System.Web;
using System.Web.Mvc;

namespace Axis_Bank_Biller_Integration_GL
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
