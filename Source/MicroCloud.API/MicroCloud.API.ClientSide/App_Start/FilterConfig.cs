using System.Web;
using System.Web.Mvc;

namespace MicroCloud.API.ClientSide
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
