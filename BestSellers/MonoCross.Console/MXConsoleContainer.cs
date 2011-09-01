using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using MonoCross.Navigation;

namespace MonoCross.Console
{
    public class MXConsoleContainer : MXContainer<MXConsoleContainer>, IMXContainer
    {
        public static Dictionary<int, string> NavigationKeys = new Dictionary<int, string>();
        public static List<string> NavHistory = new List<string>();

        public static void Initialize(MXApplication theApp)
        {
            MXContainer<MXConsoleContainer>.Initialize(new MXConsoleContainer(), theApp);
        }
        public override void  OnLoadComplete(IMXController controller, MXViewPerspective perspective)
        {
            ResetNavigationKeys();

            System.Console.WriteLine();

            if (controller.HasView)
                controller.RenderView();
            else
                MXConsoleContainer.RenderViewFromPerspective(controller, perspective);

            System.Console.WriteLine();
            System.Console.WriteLine("0. Back ...");
            System.Console.WriteLine();

            string input = System.Console.ReadLine();
            string action = MXApplication.Instance.NavigateOnLoad;

            try
            {
                int key = Convert.ToInt16(input);
                action = NavigationKeys[key];
            }
            catch
            {
                action = input;
                if (input == "")
                    action = MXApplication.Instance.NavigateOnLoad;
            }

            if (NavHistory.Contains(action))
            {
                do { NavHistory.RemoveAt(NavHistory.Count - 1); }
                while (NavHistory[NavHistory.Count - 1] != action);
            }
            else
            {
                NavHistory.Add(action);
            }

            System.Console.WriteLine();
            MXConsoleContainer.Navigate(action);
        }

        protected void ResetNavigationKeys()
        {
            NavigationKeys.Clear();
            NavigationKeys.Add(0, MXApplication.Instance.NavigateOnLoad);
            if (NavHistory.Count >= 2)
            {
                NavigationKeys.Remove(0);
                NavigationKeys.Add(0, NavHistory[NavHistory.Count - 2]);
            }
        }
        private string StripHtml(string text)
        {
            text = text.Replace("<br/>", "\n");
            text = text.Replace("</h3>", "\n");
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        public override void Redirect(string url)
        {
            Navigate(url);
            CancelLoad = true;
        }
    }    
}
