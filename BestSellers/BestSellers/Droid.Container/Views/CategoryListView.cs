using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using MonoCross.Navigation;
using MonoCross.Droid;

using BestSellers;

namespace Droid.Container
{
    [Activity(Label = "The New York Times Best Sellers")]
    public class CategoryListView : MXListActivityView<CategoryList>
    {
        public override void Render()
        {
            ListView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, Model.ToArray());
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);

            string url = string.Format(Model[position]);

            MXDroidContainer.Navigate(url, this);
        }
    }
}

