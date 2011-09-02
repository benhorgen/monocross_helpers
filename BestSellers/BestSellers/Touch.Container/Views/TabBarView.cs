using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

using MonoTouch;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

using MonoCross.Touch;
using MonoTouch.Dialog;
using MonoCross.Navigation;

namespace Touch.TestContainer.Views
{
	[MXTouchViewType(ViewType.Master)]
	public class TabBarView : MXTouchTabBarController<object>
	{
		public TabBarView() { View.Frame = new RectangleF (0, 20, 320, 460); }

		public override void Render()
		{
			int index = 0;
			var rootTabBarCtrls = new UIViewController[3];
			
			// configure a nav controller for each tab
			UINavigationController navCtrl = new UINavigationController();
			navCtrl.NavigationBar.TintColor = UIColor.Black;
			navCtrl.TabBarItem = new UITabBarItem("Hardcover Advice", UIImage.FromBundle ("images/dashboard.png"), 0);
			rootTabBarCtrls[index++] = navCtrl;			
			
			navCtrl = new UINavigationController();
			navCtrl.NavigationBar.TintColor = UIColor.Black;
			navCtrl.TabBarItem = new UITabBarItem("Mass Market Paperback", UIImage.FromBundle ("images/warning.png"), 0);
			rootTabBarCtrls[index++] = navCtrl;
			
			navCtrl = new UINavigationController();
			navCtrl.NavigationBar.TintColor = UIColor.Black;
			navCtrl.TabBarItem = new UITabBarItem("Hardcover Nonfiction", UIImage.FromBundle ("images/magnify.png"), 0);
			rootTabBarCtrls[index++] = navCtrl;
			
			// setup the tab bar
			SetViewControllers(rootTabBarCtrls, true);
			this.Delegate = new TabBarDelegate();
		}

		public override void ViewDidAppear(bool b)
		{
			SelectedIndex = 0;
			MXTouchContainer.Navigate("Books/Hardcover Advice");
		}
	}

	public class TabBarDelegate : UITabBarControllerDelegate
	{		
		public override void ViewControllerSelected(UITabBarController tabBarController, UIViewController viewController)
		{
			string uri = null;
			switch (tabBarController.SelectedIndex) 
			{
			case 0:
				uri = "Books/Hardcover Advice";
				break;
			case 1:
				uri = "Books/Mass Market Paperback";
				break;
			case 2:
				uri = "Books/Hardcover Nonfiction";
				break;
			}
			
			var x = (UINavigationController)tabBarController.ViewControllers[tabBarController.SelectedIndex];
			if (x.TopViewController == null)
			{
				new System.Threading.Thread (() => 
				{
					using (new MonoTouch.Foundation.NSAutoreleasePool()) 
					{
							MXTouchContainer.Navigate(uri);							
					}
				}).Start ();
			}
			else { Console.WriteLine ("TopViewController is not null, not navigating to: " + uri); }
		}
		
		public static string CurrentlySelectedScreen { get; set; }
	}
}


