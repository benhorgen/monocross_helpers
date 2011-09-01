using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;

using MonoCross.Navigation;
using MonoCross.Droid;

using BestSellers;

namespace Droid.Container
{
    [BroadcastReceiver]
    [IntentFilter(new string[] { "MonoCross.MainReceiver.BestSellers" })]
    public class MainReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            // initialize app
            MXDroidContainer.Initialize(new BestSellers.App());

            // initialize views
            MXDroidContainer.AddView<CategoryList>(typeof(CategoryListView), ViewPerspective.Read);
            MXDroidContainer.AddView<BookList>(typeof(Views.BookListView), ViewPerspective.Read);
            MXDroidContainer.AddView<Book>(typeof(Views.BookView), ViewPerspective.Read);

            // navigate to first view
            MXDroidContainer.Navigate(MXApplication.Instance.NavigateOnLoad, context);            
        }
    }
}