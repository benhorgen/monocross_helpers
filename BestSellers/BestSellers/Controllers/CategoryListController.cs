using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using MonoCross.Navigation;
using BestSellers;

namespace BestSellers.Controllers
{
    public class CategoryListController : MXController<CategoryList>, IMXController
    {
        public override string Load(Dictionary<string, string> parameters)
        {
            Model = new CategoryList();
            string urlCategories = "http://api.nytimes.com/svc/books/v2/lists/names.xml?api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697";
            try
            {
                XDocument loaded = XDocument.Load(urlCategories);
                var categories = from item in loaded.Descendants("result")
                                 select new
                                 {
                                     name = (string)item.Element("list_name"),
                                 };

                foreach (var category in categories)
                {
                    Model.Add(category.name);
                }
            }
            catch
            {
                Model.Add("No List retrieved");
            }
            return ViewPerspective.Read;
        }
    }
}
