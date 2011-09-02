using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using MonoCross.Navigation;
using BestSellers;
using System.Globalization;

namespace BestSellers.Controllers
{
    class BookListController : MXController<BookList>
    {
		public static string CapitalizeString(string stringValue)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stringValue.ToLower());
		}
		
        public override string Load(Dictionary<string,string> parameters)
        {            
            string category = parameters.ContainsKey("Category") ? parameters["Category"] : "BookList";

            Model = new BookList();
            Model.Category = category;

            string urlBooks = String.Format("http://api.nytimes.com/svc/books/v2/lists/{0}.xml?api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697", category.Replace(" ", "-"));
            try
            {
				Console.WriteLine("trying: " + urlBooks);
                XDocument loaded = XDocument.Load(urlBooks);
                var books = from item in loaded.Descendants("book")
                            where item.Descendants().Count() > 9
                            select new Book()
                            {
                                Category = category,
                                Title = CapitalizeString((string)item.Descendants("book_detail").Elements("title").First()),
                                Contributor = (string)item.Descendants("book_detail").Elements("contributor").First(),
                                Author = (string)item.Descendants("book_detail").Elements("author").First(),
                                ISBN10 = (string)item.Descendants("isbn").Elements("isbn10").First(),
                            };
                Model.AddRange(books);
            }
            catch (Exception e) { Console.WriteLine("Exception loading Book List\r\n: " + e.ToString()); }

            return ViewPerspective.Read;
        }
    }
}
