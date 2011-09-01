using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using MonoCross.Console;

using BestSellers;

namespace Console.Container.Views
{
    class BookListView : MXView<BookList>
    {
        public override void Render()
        {
            System.Console.WriteLine(Model.Category);
            System.Console.WriteLine();

            foreach (Book book in Model)
            {
                System.Console.WriteLine(MXConsoleContainer.NavigationKeys.Count + ".  " + book.Title);
                MXConsoleContainer.NavigationKeys.Add(MXConsoleContainer.NavigationKeys.Count, string.Format("{0}/{1}", book.Category, book.ISBN10));
            }
        }
    }
}
