using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using MonoCross.Navigation;
using MonoCross.Touch;

using BestSellers;

namespace Touch.TestContainer
{
	public class Application
	{
		static void Main (string[] args)
		{
			UIApplication.Main (args);
		}
	}

	// The name AppDelegate is referenced in the MainWindow.xib file.
	public partial class AppDelegate : UIApplicationDelegate
	{
		// This method is invoked when the application has loaded its UI and its ready to run
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			MXTouchContainer.Initialize(new BestSellers.App(), window, MXTouchNavigationStyle.Standard,  "splash.png");

			//Add some Views
			MXTouchContainer.AddView<CategoryList>(new Views.CategoryListView(), ViewPerspective.Read);
			MXTouchContainer.AddView<BookList>(new Views.BookListView(), ViewPerspective.Read);
			MXTouchContainer.AddView<Book>(new Views.BookView(), ViewPerspective.Read);
			
			MXTouchContainer.AddView<object> (new Views.TabBarView());
			
			MXTouchContainer.Navigate(MXApplication.Instance.NavigateOnLoad);
			
			return true;
		}

		// This method is required in iPhoneOS 3.0
		public override void OnActivated (UIApplication application)
		{
		}
	}
}

