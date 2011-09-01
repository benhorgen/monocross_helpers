using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MonoCross.Navigation
{
    public interface IMXContainer
    {
        void Redirect(string url);
    }
    public abstract class MXContainer<T> where T : MXContainer<T>, IMXContainer
    {
        protected bool CancelLoad = false;
        //public static event ControllerEventHandler OnLoadComplete;
        //public virtual void OnViewLoading(IMXController controller) { }
        public virtual void OnLoadComplete(IMXController controller, MXViewPerspective viewPerspective)
        {
        }

        public ViewMap Views = new ViewMap();

        #region ctor/abstract singleton initializers

        protected MXContainer()
        {
            //OnLoadComplete += (IMXController controller) => { OnViewLoading(controller); };
        }

        /// <summary>
        /// Initializes the specified target factory instance.
        /// </summary>
        /// <param name="newInstance">A <see cref="T"/> representing the target factory value.</param>
        protected static void Initialize(T newInstance)
        {
            if (newInstance == null)
                throw new ArgumentNullException();
            Instance = newInstance;
            //iApp.Factory = theFactory;
        }

        protected static void Initialize(T newInstance, MXApplication theApp)
        {
            Initialize(newInstance);
            MXApplication.Initialize(theApp, newInstance);
        }

        #region singleton accessors
        public static T Instance { get; private set; }
        #endregion

        #endregion

        public static void AddView<P>(IMXView view)
        {
            AddView<P>(view, ViewPerspective.Default);
        }
        public static void AddView<P>(IMXView view, string perspective)
        {
            Instance.Views.Add(new MXViewPerspective(typeof(P), perspective), view);
        }
        public static void AddView<P>(Type viewType)
        {
            AddView<P>(viewType, ViewPerspective.Default);
        }
        public static void AddView<P>(Type viewType, string perspective)
        {
            Instance.Views.Add(new MXViewPerspective(typeof(P), perspective), viewType);
        }
        
        #region navigation methods
        public static MXNavigation MatchUrl(string url)
        {
            MXNavigation navMap = MXApplication.NavigationMap.Where(pattern => Regex.Match(url, pattern.RegexPattern()).Value == url).FirstOrDefault();
            return navMap;
        }
        public static IMXController Navigate(string url)
        {
            return Navigate(url, new Dictionary<string, string>());
        }
        public static IMXController Navigate(string url, Dictionary<string, string> parameters)
        {
            return Instance.GetController(url, parameters);
        }
        public abstract void Redirect(string url);

        protected virtual IMXController GetController(string url, Dictionary<string, string> parameters)
        {
            IMXController controller = null;
            MXNavigation navigation = null;

            // return if no url provided
            if (url == null)
                throw new ArgumentException("url is NULL");

            // set last navigation
            MXApplication.LastNavigationDate = DateTime.Now;

            // initialize parameter dictionary if not provided
            parameters = (parameters ?? new Dictionary<string, string>());

            // for debug
            Console.WriteLine("Navigating to: " + url);

            // get map object
            navigation = MatchUrl(url);

            // If there is no result, assume the URL is external and create a new Browser View
            if (navigation != null)
            {
                controller = navigation.Controller;

                // Now that we know which mapping the URL matches, determine the parameter names for any Values in URL string
                Match match = Regex.Match(url, navigation.RegexPattern());
                MatchCollection args = Regex.Matches(navigation.Pattern, @"{(?<Name>\w+)}*");

                // If there are any parameters in the URL string, add them to the parameters dictionary
                if (match != null && args.Count > 0)
                {
                    foreach (Match arg in args)
                    {
                        if (parameters.ContainsKey(arg.Groups["Name"].Value))
                            parameters.Remove(arg.Groups["Name"].Value);
                        parameters.Add(arg.Groups["Name"].Value, match.Groups[arg.Groups["Name"].Value].Value);
                    }
                }

                //Add default view parameters without overwriting current ones new comment here
                if (navigation.Parameters.Count > 0)
                {
                    foreach (KeyValuePair<string, string> param in navigation.Parameters)
                    {
                        if (!parameters.ContainsKey(param.Key))
                        {
                            parameters.Add(param.Key, param.Value);
                        }
                    }
                }
            }
#if DEBUG
            else { throw new ApplicationException("URI match not found for: " + url); }
#endif

            // Initiate load for the associated controller passing all parameters 
            if (controller != null)
            {
                CancelLoad = false;
                
                // allow the model to it's Model, if failed it should initiate a seperate Navigation
                string perspective = controller.Load(parameters);

                if (!CancelLoad)
                {
                    // check if failed
                    MXViewPerspective viewKey = new MXViewPerspective(controller.ModelType, perspective);
                    if (Instance.Views.ContainsKey(viewKey))
                    {
                        controller.View = Instance.Views[viewKey];
                        controller.View.SetModel(controller.GetModel());
                    }
                    //if (!controller.HasOnLoadCompleteDelegate)
                    //    controller.OnLoadComplete += (IMXController pcontroller) => { ControllerLoadComplete(pcontroller); };
                    OnLoadComplete(controller, viewKey);
                }
            }
            return controller;
        }
        #endregion

        public static void RenderViewFromPerspective(IMXController controller, MXViewPerspective perspective)
        {
            Instance.Views.RenderView(perspective, controller.GetModel());
        }

        private static void NotifyViewModelChanged(IMXView view, object model)
        {
            foreach (KeyValuePair<MXViewPerspective, IMXView> entry in Instance.Views)
            {
                if (view != entry.Value)
                    entry.Value.OnViewModelChanged(model);
            }
        }

        public class ViewMap : Dictionary<MXViewPerspective, IMXView>
        {
            private Dictionary<MXViewPerspective, Type> typeMap = new Dictionary<MXViewPerspective, Type>();
            
            public new void Add(MXViewPerspective perspective, Type viewType)
            {
                if (!viewType.GetInterfaces().Contains(typeof(IMXView)))
                    throw new ArgumentException("Type provided does not implement IMXView interface.", "viewType");
                typeMap.Add(perspective, viewType);
            }
            public new void Add(MXViewPerspective perspective, IMXView view)
            {
                base.Add(perspective, view);
                view.ViewModelChanged += delegate(object model)
                {
                    NotifyViewModelChanged(view, model);
                };
            }
            
            public Type GetViewType(MXViewPerspective viewPerspective) 
            {
                Type type;
                typeMap.TryGetValue(viewPerspective, out type);
                return type;
            }
            public IMXView GetView(MXViewPerspective viewPerspective) 
            {
                IMXView view;
                TryGetValue(viewPerspective, out view);
                return view;
            }
            
            internal virtual void RenderView(MXViewPerspective perspective, object model)
            {
                IMXView view;
                if (this.TryGetValue(perspective, out view))
                {
                    view.SetModel(model);
                    view.Render();
                }
                else if (typeMap.ContainsKey(perspective))
                {
                    view = (IMXView)typeMap[perspective].GetConstructor(Type.EmptyTypes).Invoke(new object[] { }); 
                    view.SetModel(model);
                    view.Render();
                }
            }
        }
    }
}