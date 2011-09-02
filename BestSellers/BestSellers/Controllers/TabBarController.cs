using System.Collections.Generic;
using MonoCross.Navigation;

namespace BestSellers.Controllers
{
    /// <summary>Destination controller, controller for 3 views: CountryList, StateList, City List</summary>
    public class TabBarController: MXController<object>, IMXController
    {
        public TabBarController()
        {
            Model = new object();
        }
        
        public override string Load(Dictionary<string, string> parameters)
        {
            return ViewPerspective.Default;
        }			
    }
}