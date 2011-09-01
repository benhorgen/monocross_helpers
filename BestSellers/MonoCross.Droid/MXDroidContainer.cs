using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;

using MonoCross.Navigation;

namespace MonoCross.Droid
{
    public class MXDroidContainer : MXContainer<MXDroidContainer>, IMXContainer
    {
        // TODO: find a way that doesn't feel like it brake M-V-C
        public static Dictionary<Type, object> ViewModels = new Dictionary<Type, object>();
        public static Action<Type> NavigationHandler { get; set; }
        private static Context Context { get; set; }

        public static void Initialize(MXApplication theApp)
        {
            MXContainer<MXDroidContainer>.Initialize(new MXDroidContainer(), theApp);
        }

        public static void Navigate(string url, Context context)
        {
            Context = context;
            Navigate(url);
        }

        public static void Navigate(string url, Dictionary<string, string> parameters, Context context)
        {
            Context = context;
            Navigate(url, parameters);
        }

        public override void OnLoadComplete(IMXController controller, MXViewPerspective viewPerspective)
        {
            Type viewType = Views.GetViewType(viewPerspective);

            if (viewType != null)
            {
                //viewType = Instance.ViewTypeMap[controller.ModelType];

                // stash the model away so we can get it back when the view shows up!
                ViewModels[controller.ModelType] = controller.GetModel();

                // construct the intent to go to our view
                if (NavigationHandler == null)
                {
                    Intent intent = new Intent(Context, viewType);
                    intent.AddFlags(ActivityFlags.NewTask);
                    Context.StartActivity(intent);
                }
                else
                {
                    NavigationHandler(viewType);
                }
            }
            else { throw new TypeLoadException("View not found for " + viewPerspective.ToString()); }
        }
        public override void Redirect(string url)
        {
            Navigate(url, Context);
            CancelLoad = true;
        }
    }
}