using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCross.Navigation
{
    public abstract class MXApplication
    {
        protected static MXApplication theApp;
        protected IMXContainer theContainer;

        #region properties
        public string NavigateOnLoad { get; set; }
        public string Title { get; set; }
        public static NavigationList NavigationMap = new NavigationList();
        public static DateTime LastNavigationDate { get; set; }
        #endregion

        #region ctor/initialization
        protected MXApplication()
        {
            NavigateOnLoad = string.Empty;

            OnAppLoad();
        }
        public static void Initialize(MXApplication theApp, IMXContainer theContainer)
        {
            lock (typeof(MXApplication))
            {
                Instance = theApp;
                Container = theContainer;
            }

        }
        #endregion

        #region virtual event methods
        public virtual void OnAppLoad() { }
        public virtual void OnAppLoadComplete() { }
        #endregion

        #region singleton accessors
        public static MXApplication Instance { get { return theApp; } private set { theApp = value; } }
        public static IMXContainer Container { get { return Instance.theContainer; } private set { Instance.theContainer = value; } }
        #endregion

        public static void Redirect(string url)
        {
            Container.Redirect(url);
        }

        #region application classes
        public class NavigationList : List<MXNavigation>
        {
            public void Add(string pattern, IMXController controller)
            {
                this.Add(pattern, controller, new Dictionary<string, string>());
            }

            public void Add(string pattern, IMXController controller, Dictionary<string, string> parameters)
            {
                // Enforce uniqueness
                MXNavigation currentMatch = null;
                if (this.Where(m => m.Pattern == pattern).Count() > 0)
                {
                    currentMatch = this.Where(m => m.Pattern == pattern).First();
                }
                if (currentMatch != null)
                {
#if DEBUG
                    string text = string.Format("MapUri \"{0}\" is already matched to Controller type {1}",
                                                                            pattern, currentMatch.Controller);
                    throw new ApplicationException(text);
#else
                    return;
#endif
                }

                this.Add(new MXNavigation(pattern, controller, parameters));
            }
        }

        #endregion
    }
}