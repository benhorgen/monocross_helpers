using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MonoCross.Navigation;
using MonoCross.Webkit;

using BestSellers;

namespace Webkit.Container.Controllers
{
    [HandleError]
    public class AppController : Controller
    {
        public ActionResult Render(string mapUri)
        {
            IMXController controller = MXWebkitContainer.Navigate(mapUri == null ? MXApplication.Instance.NavigateOnLoad : mapUri, this.Request);

            if (controller != null)
            {
                if (controller.ModelType == typeof(CategoryList))
                    return View(((MXController<CategoryList>)controller).Model);
                else
                {
                    controller.RenderView();
                    return null;
                }
            }
            return null;
        }
    }
}
