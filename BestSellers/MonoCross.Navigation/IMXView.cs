using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCross.Navigation
{
    #region IMXView interface
    public interface IMXView
    {
        Type ModelType { get; }
        void Render();
    }
    #endregion

    public abstract class MXView<T> : IMXView
    {
        public T Model { get; set; }
        public Type ModelType { get { return typeof(T); } }
        public abstract void Render();        
    }
}
