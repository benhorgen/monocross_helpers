using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCross.Navigation
{
    public class MXNavigation
    {
        #region properties
        public IMXController Controller { get; set; }
        public string Pattern { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        #endregion

        #region ctors
        public MXNavigation(string pattern, IMXController controller, Dictionary<string, string> parameters)
        {
            Controller = controller;
            Pattern = pattern;
            Parameters = parameters;
        }
        #endregion

        #region methods
        public string RegexPattern()
        {
            return Pattern.Replace("{", "(?<").Replace("}", @">[-&\w\. ]+)");
        }
        #endregion
    }    
}
