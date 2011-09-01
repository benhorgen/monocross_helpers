using System;
using System.Collections.Generic;

using MonoCross.Navigation;
using MonoCross.Touch;
using MonoTouch.UIKit;

namespace MonoCross.Touch
{
	public enum MXTouchNavigationStyle
	{
		Standard,
		SplitViewOnPad
	}
	
	public class MXTouchContainer: MXContainer<MXTouchContainer>, IMXContainer
	{
		static TouchNavigationHelper _touchNavigation;
		static UIWindow _window;

		public static Dictionary<Type, UIViewController> ViewControllers;
		
		private MXTouchContainer () : base() { }
		
		private static ViewType getViewType(Type view)
		{
			// Show MyCustomAttribute information for the testClass
			MXTouchViewType attr = Attribute.GetCustomAttribute(view, typeof(MXTouchViewType)) as MXTouchViewType;
			if (attr != null)
				return attr.ViewType;
			else
				return ViewType.Detail;
		}
		
		public static void Initialize(MXApplication theApp, UIWindow window, MXTouchNavigationStyle navStyle, string splashBitmapFile)
		{
            MXContainer<MXTouchContainer>.Initialize(new MXTouchContainer(), theApp);

            ViewControllers = new Dictionary<Type, UIViewController>();

			_touchNavigation = new TouchNavigationHelper(navStyle, splashBitmapFile);
			_window = window;
			if (_window.Subviews.Length == 0)
			{
				// toss in a temporary view until async initialization is complete
				_window.AddSubview(new SplashViewController(splashBitmapFile).View);
				_window.MakeKeyAndVisible();
			}
		}
		
		public static bool IsSplitView { get { return _touchNavigation.IsSplit; }}
		public static bool HasTabBarControllerAsRoot { get { return _touchNavigation.HasTabBar; }}
		
		static bool _firstView = true;

		private static void ShowView ()
		{
			if (_firstView)
			{
				foreach (var view in _window.Subviews)
					view.RemoveFromSuperview();
				
				_firstView = false;
				_window.Add(_touchNavigation.View);
				_window.MakeKeyAndVisible();
			}
		}
		
		public new static IMXController Navigate(string url)
		{
			return Navigate(url, null);
		}

		public new static IMXController Navigate(string url, Dictionary<string, string> parameters)
		{
            // start the lower level navigation machinary
			IMXController controller = MXContainer<MXTouchContainer>.Navigate(url, parameters);
			
			return controller;
		}
		
		public static IMXController Navigate(string url, MXTouchAnimation direction)
		{
			IMXController controller = MXContainer<MXTouchContainer>.Navigate(url);
			
			return controller;
		}
		
		public override void OnLoadComplete (IMXController controller, MXViewPerspective viewPerspective)
		{
			ViewType vt = ViewType.Master;
			IMXView view = Instance.Views.GetView(viewPerspective);
			if (null == view)
			{
				Type type = Instance.Views.GetViewType(viewPerspective);
				if (type != null) 
				{
					vt = getViewType(type);
					
					//TODO: Instantiate an instance of that type an assign to the view
					//view = new typeof(Type)();
					
					//TODO: Call Render on new view
					//view.Render();
				}
			}
			else
			{
				view.Render();
				vt = getViewType(view.GetType());
			}
			
			UIViewController vc = view as UIViewController;
			if (HasTabBarControllerAsRoot)
			{
				_touchNavigation.DisplayViewInTabBar(vc, false);
			}
			else
			{
				switch (vt)
				{
				case ViewType.Detail:
					_touchNavigation.NavigateToDetail(vc);
					break;
				case ViewType.Master:
					_touchNavigation.NavigateToMaster(vc);
					break;
				case ViewType.Popover:
	                _touchNavigation.NavigateToPopover(vc, true);
					break;
				}
			}
			
            // handle initial view display if not already handled
			ShowView();
		}
		
        public override void Redirect(string url)
        {
            Navigate(url);
            CancelLoad = true;
        }
	}
}

