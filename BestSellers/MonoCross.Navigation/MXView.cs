using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCross.Navigation
{
	public delegate void ModelEventHandler(object model);
	
    #region IMXView interface
    public interface IMXView
    {
        // the only reason we have these two is because Mono does non yet
        // fully support covariance.  So if you'd like to tackle that
        // drinks are on me ;^)
        Type ModelType { get; }
        void SetModel(object model);

        void Render();
		event ModelEventHandler ViewModelChanged;
		void OnViewModelChanged(object model);
    }
    #endregion

    public abstract class MXView<T> : IMXView
    {
        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();
        public void SetModel(object model)
        {
            Model = (T)model;
        }
		public event ModelEventHandler ViewModelChanged;		
		public virtual void OnViewModelChanged(object model) { }
		public void NotifyModelChanged() { if (ViewModelChanged != null) ViewModelChanged(this.Model); } 
    }
}
