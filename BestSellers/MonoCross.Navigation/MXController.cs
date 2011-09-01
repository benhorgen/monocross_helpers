using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCross.Navigation
{
    #region events
    public delegate void ControllerEventHandler(IMXController controller);
    #endregion

    #region IMXController interface
    public interface IMXController
    {
        //event ControllerEventHandler OnLoadComplete;

        //bool CancelLoad { get; set; }
		//bool HasOnLoadCompleteDelegate { get; }
        bool HasView { get; }
        IMXView View { get; set; }

        // the only reason we have these two is because Mono does non yet
        // fully support covariance.  
        //So if you'd like to tackle that,
        // drinks are on me ;^)
        Type ModelType { get; }
        object GetModel();

        string Load(Dictionary<string, string> parameters);
        void RenderView();
    }
    #endregion

    public abstract class MXController<T> : IMXController
    {
        #region events
        //public event ControllerEventHandler OnLoadComplete;
        #endregion

        #region properties
        //public bool CancelLoad { get; set; }
        public string MapUri { get; set; }
        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
		//public bool HasOnLoadCompleteDelegate { get { return OnLoadComplete != null; } }
        public bool HasView { get { return this.View != null; } }

        public virtual IMXView View { get; set; }
        #endregion

        #region methods
        public object GetModel() { return Model; }
        public abstract string Load(Dictionary<string, string> parameters);
        public virtual void RenderView() { if (HasView) View.Render(); }
        #endregion
    }
}
