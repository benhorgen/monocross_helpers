using System;
using MonoTouch.UIKit;

namespace MonoCross.Touch
{
    /// <summary>
    /// Class to display the initial view when still warming up
    /// </summary>
	internal class SplashViewController: UIViewController
	{
		public SplashViewController (string imageFile)
		{
			UIImage image = null;
			if (!String.IsNullOrEmpty(imageFile))
				image = UIImage.FromFile(imageFile);

			UIImageView imageView;
			if (image != null)
				imageView = new UIImageView(image);
			else
				imageView = new UIImageView();
			imageView.ContentMode = UIViewContentMode.Center;
			
			this.View = imageView;
		}

		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);
			
			/*
			UIImageView view = this.View as UIImageView;
			
			if (toInterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || 
			    toInterfaceOrientation == UIInterfaceOrientation.LandscapeRight)
				view.Image = UIImage.FromFile("Images/Launch-Landscape.png");
			else
				view.Image = UIImage.FromFile("Images/Launch-Portrait.png");
			*/
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}

