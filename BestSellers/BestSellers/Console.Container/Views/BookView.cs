﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoCross.Navigation;
using MonoCross.Console;

using BestSellers;


namespace Console.Container.Views
{
    class BookView : MXView<Book>
    {
        public override void Render()
        {
            System.Console.WriteLine("Book Details");
            System.Console.WriteLine();

            System.Console.WriteLine(Model.Title);
            System.Console.WriteLine(Model.Contributor);
            System.Console.WriteLine(string.Format("${0}", Model.Price));
            System.Console.WriteLine();
            System.Console.WriteLine(Model.Description);
            System.Console.WriteLine();
        }
    }
}
