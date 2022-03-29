using System.Web;
using System.Web.Mvc;

namespace Proyecto_Funeraria2020
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
