using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using BestSellers.Controllers;

namespace BestSellers
{
    public class App : MXApplication
    {
        public override void OnAppLoad()
        {
            // Set the application title
            Title = "Best Sellers";

            // Add navigation mappings
            NavigationMap.Add("", new CategoryListController());
            NavigationMap.Add("Books/{Category}", new BookListController());
            NavigationMap.Add("Books/{Category}/{Book}", new BookController());
			
			// Adding a naviagation uri for the MainTabs
			NavigationMap.Add("MainTabs", new TabBarController());

            // Set the navigation URI to initialize the application's tabs
            NavigateOnLoad = "MainTabs";
        }
    }
}
