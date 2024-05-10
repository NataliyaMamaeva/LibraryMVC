using LibraryMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibraryMVC.Controllers
{
    public class LibraryController : Controller
    {
        public LibraryContext context = new();
        private readonly ILogger<LibraryController> _logger;
        public LibraryController(ILogger<LibraryController> logger) { _logger = logger; }

        public IActionResult Index()   { return View(); }
        public IActionResult Search() { return PartialView(); }
        public IActionResult Redactor() { return PartialView(); }
        public IActionResult Settings() { return PartialView(); }
        public ActionResult GivePopup() { return PartialView(""); }

        public SettingsLog PutSettings([FromBody] JsonElement data)
        {
            Console.WriteLine("put settings request");
            Console.WriteLine(data);
            var settingsData = JsonConvert.DeserializeObject<SettingsLog>(data.GetRawText());
            var set = context.Settings.ToArray()[0];
            set.days = settingsData.days;
            set.lateReaction = settingsData.lateReaction;
            context.SaveChanges();
            return set;
        }

        public List<Book> GetBookByTitle()
        {
            Console.WriteLine("GetBookByTitle request");
            if (Request.QueryString.HasValue)
            {
                string title = Request.Query["title"];
                Console.WriteLine(title);
                var books = new List<Book>();
                foreach(var book in context.Books)
                { 
                    if (book.Title.Contains(title) )
                    {  
                        books.Add(book);
                        Console.WriteLine(book.Title);
                    }
                }
                return books;
            }
            else  {
                var books = context.Books.Where(book => book.readerId == null).ToList();
                return books; 
            }
        }
        public List<Book> GetBookByAuthor()
        {
            Console.WriteLine("GetBookByAuthor request");
            if (Request.QueryString.HasValue)
            {
                string authorName = Request.Query["authorName"];
                Console.WriteLine(authorName);
                var books = new List<Book>();
                foreach (var book in context.Books)
                {
                    if (book.authorName.Contains(authorName))
                    {
                        books.Add(book);
                        Console.WriteLine(book.Title);
                    }
                }
                return books;
            }
            else
            { return context.Books.ToList(); }
        }
        public List<Author> GetAuthorByName()
        {
            Console.WriteLine("GetAuthorByName request");
            if (Request.QueryString.HasValue)
            {
                string name = Request.Query["name"];
                Console.WriteLine(name);
                var authors = new List<Author>();
                foreach (var a in context.Authors)
                {
                    if (a.Name.Contains(name)) { Console.WriteLine(a.Name); authors.Add(a); }
                }
                return authors;
            }
            else { return context.Authors.ToList(); }
        }

        public Author GetAuthorById()
        {
           int id;     
           Int32.TryParse(Request.Query["id"], out id);
           Author author = context.Authors.FirstOrDefault(a => a.Id == id);
           return author;
        }

        public List<Genre> GetGenres()
        {
            Console.WriteLine("GetGenres request");
             return context.Genres.ToList(); 
        }

        public Genre GetGenreById()
        {
            int id;
            Int32.TryParse(Request.Query["id"], out id);
            var genre = context.Genres.FirstOrDefault(a => a.Id == id);
            return genre;
        }
        public Language GetLanguageById()
        {
            int id;
            Int32.TryParse(Request.Query["id"], out id);
            var lang = context.Languages.FirstOrDefault(a => a.Id == id);
            return lang;
        }
        public List<Language> GetLanguages()
        {
            Console.WriteLine("GetLanguages request");
            var langs = context.Languages.ToList();
            foreach (var language in langs)
            {
                Console.WriteLine(language.language);
            }

            return context.Languages.ToList();
        }

        public List<Book> GetBookBySelect()
        {
            int genreId;
            Int32.TryParse(Request.Query["genre"], out genreId);
            //string genreId = Request.Query["genre"];
            int languageId;
            Int32.TryParse(Request.Query["language"], out languageId);
            //string languageId = Request.Query["language"];
            string status = Request.Query["status"].ToString();
            List<Book> books = new List<Book>();
            Console.WriteLine($"GetBookBySelect request. genre: {genreId}; language: {languageId};status: {status}.");


            foreach (var book in context.Books)
            {
                if (genreId == 0)
                {
                    if(languageId == 0)
                        books.Add(book);
                    else
                    {
                        if(book.languageId == languageId) books.Add(book);
                    }
                }
                else
                {
                    if (languageId == 0)
                    {
                        if(book.genreId == genreId) books.Add(book);
                    }
                    else
                    {
                        if (book.genreId == genreId && book.languageId == languageId)
                        {
                            //Console.WriteLine(book.Title);
                            books.Add(book);
                        }
                    }
                }
            }
            if (status == "available")
            {
                for (int i = 0; i < books.Count();)
                {
                    if (books[i].readerId != null)
                        books.RemoveAt(i);
                    else i++;
                }
            }
            return books;
        }

        public Genre PostGenre([FromBody] JsonElement data)
        {
            Console.WriteLine("post genre request");
            Console.WriteLine(data);
            var genre = JsonConvert.DeserializeObject<Genre>(data.GetRawText());
            Console.WriteLine(genre);
            context.Genres.Add(genre);
            context.SaveChanges();
            return genre;
        }
        public Language PostLanguage([FromBody] JsonElement data)
        {
            Console.WriteLine("post language request");
            Console.WriteLine(data);
            var language = JsonConvert.DeserializeObject<Language>(data.GetRawText());
            Console.WriteLine(language);
            context.Languages.Add(language);
            context.SaveChanges();
            return language;
        }
        public Author PostAuthor([FromBody] JsonElement data)
        {
            Console.WriteLine("post author request");
            Console.WriteLine(data);
            var author = JsonConvert.DeserializeObject<Author>(data.GetRawText());
            Console.WriteLine(author);
            foreach (Author a in context.Authors)
                if (a.Name == author.Name)
                    return new Author { Name = null };
            context.Authors.Add(author);
            context.SaveChanges();
            return author;
        }
        public Book PostBook([FromBody] JsonElement data)
        {
            Console.WriteLine("post book request");
            Console.WriteLine(data);
            var book = JsonConvert.DeserializeObject<Book>(data.GetRawText());
            Console.WriteLine(book);
            string author = context.Authors.FirstOrDefault(a => a.Id == book.authorId).Name;

            //Console.WriteLine(author);
            book.authorName = author;

            context.Books.Add(book);
            context.SaveChanges();
            return book;
        }
        public Book PutBook([FromBody] JsonElement data)
        {
            Console.WriteLine("put book request");
            Console.WriteLine(data);
            var bookData = JsonConvert.DeserializeObject<Book>(data.GetRawText());
            Book book = context.Books.FirstOrDefault(b => b.Id == bookData.Id);
            book.Title = bookData.Title;
            book.year = bookData.year;
            book.authorId = bookData.authorId;
            book.genreId = bookData.genreId;
            book.languageId = bookData.languageId;
            book.authorName = context.Authors.FirstOrDefault(a => a.Id == book.authorId).Name;

            context.SaveChanges();
            return book;
        }

        public Author PutAuthor([FromBody] JsonElement data)
        {
            Console.WriteLine("put author request");
            Console.WriteLine(data);
            var authorData = JsonConvert.DeserializeObject<Author>(data.GetRawText());
            var author = context.Authors.FirstOrDefault(a => a.Id == authorData.Id);

            author.Name = authorData.Name;
            author.dateOfBirth = authorData.dateOfBirth;
            author.country = authorData.country;

            context.SaveChanges();
            return author;
        }


        public bool DeleteBook()
        {
            int id;
            Int32.TryParse(Request.Query["id"], out id);
            var book = context.Books.FirstOrDefault(a => a.Id == id);
            context.Books.Remove(book);
            context.SaveChanges();
            return true;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
