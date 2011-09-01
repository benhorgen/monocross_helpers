using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Globalization;
using System.Net;

namespace BestSellers
{
    public class Book
    {
        public string Category { get; set; }
        public string Title { get; set; }
        public string Contributor { get; set; }
        public string Author { get; set; }
        public string Rank { get; set; }
        public string BestSellersDate { get; set; }
        public string PublishedDate { get; set; }
        public string WeeksOnList { get; set; }
        public string RankLastWeek { get; set; }
        public string Description { get; set; }
        public string ContributorNote { get; set; }
        public string Price { get; set; }
        public string AgeGroup { get; set; }
        public string Publisher { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public string BookReviewLink { get; set; }
        public string FirstChapterLink { get; set; }
        public string SundayReviewLink { get; set; }
        public string ArticleChapterLink { get; set; }

        public Book() { }

		public static string CapitalizeString(string stringValue)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stringValue.ToLower());
		}

		public static Book Find(string category, string bookId)
        {
            string urlBooks = "http://api.nytimes.com/svc/books/v2/lists.xml?list={0}&isbn={1}&api-key=d8ad3be01d98001865e96ee55c1044db:8:57889697";

            urlBooks = String.Format(urlBooks, category.Replace(" ", "-"), bookId);
			
			XDocument loaded = null;
			try
			{
				loaded = XDocument.Load(urlBooks);
			}
			catch (WebException)
			{
			}
			
			Book book = null;
			if (loaded != null)
			{
				var books = from item in loaded.Descendants("book")
                    select new Book()
                    {
                        Category = category,
                        Rank = (string)item.Element("rank"),
                        BestSellersDate = (string)item.Element("bestsellers_date"),
                        PublishedDate = (string)item.Element("published_date"),
                        WeeksOnList = (string)item.Element("weeks_on_list"),
                        RankLastWeek = (string)item.Element("rank_last_week"),
                        Title = (string)item.Descendants("book_detail").Elements("title").FirstOrDefault(),
                        Description = (string)item.Descendants("book_detail").Elements("description").FirstOrDefault(),
                        Contributor = (string)item.Descendants("book_detail").Elements("contributor").FirstOrDefault(),
                        Author = (string)item.Descendants("book_detail").Elements("author").FirstOrDefault(),
                        ContributorNote = (string)item.Descendants("book_detail").Elements("contributor_note").FirstOrDefault(),
                        Price = (string)item.Descendants("book_detail").Elements("price").FirstOrDefault(),
                        AgeGroup = (string)item.Descendants("book_detail").Elements("age_group").FirstOrDefault(),
                        Publisher = (string)item.Descendants("book_detail").Elements("publisher").FirstOrDefault(),
                        ISBN10 = (string)item.Descendants("isbn").Elements("isbn10").FirstOrDefault(),
                        ISBN13 = (string)item.Descendants("isbn").Elements("isbn13").FirstOrDefault(),
                        BookReviewLink = (string)item.Descendants("review").Elements("book_review_link").FirstOrDefault(),
                        FirstChapterLink = (string)item.Descendants("review").Elements("first_chapter_link").FirstOrDefault(),
                        SundayReviewLink = (string)item.Descendants("review").Elements("sunday_review_link").FirstOrDefault(),
                        ArticleChapterLink = (string)item.Descendants("review").Elements("article_chapter_link").FirstOrDefault()
                    };
				book = (Book)books.Take(1).FirstOrDefault();
			}

			if (book == null)
			{
				// not found for whatever reason
				book = new Book();
				book.AgeGroup = string.Empty;
				book.ArticleChapterLink = string.Empty;
				book.BestSellersDate = string.Empty;
				book.BookReviewLink = string.Empty;
				book.Category = string.Empty;
				book.Contributor = string.Empty;
				book.ContributorNote = string.Empty;
				book.Description = string.Empty;
				book.FirstChapterLink = string.Empty;
				book.ISBN10 = string.Empty;
				book.ISBN13 = string.Empty;
				book.Price = string.Empty;
				book.PublishedDate = string.Empty;
				book.Publisher = string.Empty;
				book.Rank = string.Empty;
				book.RankLastWeek = string.Empty;
				book.SundayReviewLink = string.Empty;
				book.Title = string.Empty;
				book.WeeksOnList = string.Empty;				
			}
			return book;
        }
    }
}
