using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using MonoCross.Navigation;
using MonoCross.Webkit;

using BestSellers;

namespace Webkit.Container.Views
{
    public class BookListView : MXView<BookList>
    {
        public override void Render()
        {
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "iMenu");
            div.Controls.Add(new HtmlGenericControl("h3") { InnerText = Model.Category });

            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "iArrow");

            foreach (Book book in Model)
            {
                HtmlGenericControl li = new HtmlGenericControl("li");
                HtmlGenericControl a = new HtmlGenericControl("a");
                a.Attributes.Add("href", HttpUtility.UrlPathEncode(string.Format("{0}/{1}", book.Category, book.ISBN10)));
                a.Attributes.Add("rev", "async");
                HtmlGenericControl img = new HtmlGenericControl("img");
                img.Attributes.Add("src", string.Format("http://images.amazon.com/images/P/{0}.01.THUMBZZZ.png", book.ISBN10));
                img.Attributes.Add("style", "max-height:44px;max-width:32px");
                HtmlGenericControl em = new HtmlGenericControl("em");
                em.InnerText = book.Title;
                HtmlGenericControl small = new HtmlGenericControl("small");
                small.InnerText =  book.Contributor;                

                a.Controls.Add(img);
                a.Controls.Add(em);
                a.Controls.Add(small);
                li.Controls.Add(a);
                ul.Controls.Add(li);
            }
            div.Controls.Add(ul);
            MXWebkitContainer.WriteControlToResponse("BookList", Model.Category, div);
        }
    }
}