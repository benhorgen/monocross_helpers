using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Console;
using MonoCross.Navigation;

using BestSellers;

namespace Console.Container.Views
{
    public class CategoryListView : MXView<CategoryList>
    {
        public override void Render()
        {
            System.Console.WriteLine("Categories");
            System.Console.WriteLine();

            foreach (string category in Model)
            {
                System.Console.WriteLine(MXConsoleContainer.NavigationKeys.Count + ".  " + category);
                MXConsoleContainer.NavigationKeys.Add(MXConsoleContainer.NavigationKeys.Count, category);
            }
        }
    }
}
