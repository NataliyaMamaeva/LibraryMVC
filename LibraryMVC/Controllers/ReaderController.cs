using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.Entity;

namespace LibraryMVC.Controllers
{
    public class ReaderController : Controller
    {

        public LibraryContext context = new();

        public ActionResult ReaderIndex() { return View(); }
        public ActionResult Redactor()  { return PartialView("Redactor"); }
        public ActionResult Search()   { return PartialView("Search"); }
        public ActionResult Journal()   { return PartialView("Journal"); }
        public ActionResult GivePopup() { return PartialView("GivePopup"); }

        [HttpPost]
        public Reader PostReader([FromBody] JsonElement data)
        {
            Console.WriteLine("post request");
            Console.WriteLine(data);
            var reader = JsonConvert.DeserializeObject<Reader>(data.GetRawText());
            Console.WriteLine(reader);
            context.Readers.Add(reader);
            context.SaveChanges();
            return reader; 
        }

        [HttpPut]
        public Reader PutReader([FromBody] JsonElement data)
        {
            Console.WriteLine("put request");
            Console.WriteLine(data);
            var readerData = JsonConvert.DeserializeObject<Reader>(data.GetRawText());
            Console.WriteLine(readerData);
            Reader reader = context.Readers.FirstOrDefault(r => r.Id == readerData.Id);
            reader.Name = readerData.Name;
            reader.BirthDate = readerData.BirthDate;
            reader.Phone = readerData.Phone;
            context.SaveChanges();
            return reader;
        }
        public bool DeleteReader()
        {
            int id;
            Int32.TryParse(Request.Query["id"], out id);
            Console.WriteLine($" delete reader request. id: {id}");
            var reader = context.Readers.FirstOrDefault(x => x.Id == id);
            if (reader != null)
                context.Readers.Remove(reader);
            else
                return false;
            context.SaveChanges();
            return true;
        }
        public JournalLog ReturnBook()
        {
            int id;
            Int32.TryParse(Request.Query["id"], out id);
            Console.WriteLine(Request.Query["id"]);
           Console.WriteLine($"returnBook request [{id}]");
            var log = context.Journal.FirstOrDefault(l => l.bookId == id);
            var book = context.Books.FirstOrDefault(b => b.Id == log.bookId);
            book.readerId = null;
            log.returnDate = DateOnly.FromDateTime(DateTime.Now);
            context.SaveChanges();
            return log;
        }

        public List<Book> GetReaderBooks()
        {
            List<Book> books = new List<Book>();
            int id;
            Int32.TryParse(Request.Query["id"], out id);
            Console.WriteLine($"get reader books request by id: {id}");
            foreach (var book in context.Books) {
                if (book.readerId == id)
                {
                    Console.WriteLine(book.Title);
                    books.Add(book);
                }
            }
            return books;
        }
        public List<Reader> GetReaderByName()
        {
            List<Reader> readers = new List<Reader>();           
            if (Request.QueryString.HasValue)
            {
                string name =  Request.Query["name"];
                Console.WriteLine($" get request {name}");
                foreach (Reader r  in context.Readers)   
                { if (r.Name.Contains(name)) readers.Add(r); }
                return readers;     
            }
            return context.Readers.ToList();
        }
        public Reader GetReaderById()
        {
            int id;             
            Int32.TryParse(Request.Query["id"], out id);
            Console.WriteLine($" get request {id}");
            Reader reader = context.Readers.FirstOrDefault(r => r.Id == id);
            if (reader == null)
                return new Reader() { Name = null };
            return reader;
        }

        public JournalLog GiveBook([FromBody] JsonElement data)
        {
            Console.WriteLine("giveBook request");
            Console.WriteLine(data);
            ///var logData = JsonConvert.DeserializeObject<T>(data.GetRawText());
            JournalLog log = new JournalLog();
            int readerId;
            int bookId;

            Int32.TryParse(data.GetProperty("readerId").GetString(), out readerId);
            Int32.TryParse(data.GetProperty("bookId").GetString(), out bookId);

            log.readerId = readerId;
            log.bookId = bookId;

            Book book = context.Books.FirstOrDefault(b => b.Id == log.bookId);

            //if(book.readerId != null)  {   return null; }

            Reader reader = context.Readers.FirstOrDefault(r => r.Id == log.readerId);
            if (book.readerId != null)
                return new JournalLog() { bookTitle = null };
            book.readerId = reader.Id;
            log.bookTitle = book.Title;
            log.readerName = reader.Name; data.GetProperty("bookId");
            log.giveDate = DateOnly.FromDateTime(DateTime.Now);
            context.Journal.Add(log);
            context.SaveChanges();
            return log;
        }

        public List<JournalLog> GetJournal() {
            foreach (JournalLog journalLog in context.Journal)
            {
                if (journalLog.isLate) continue;
                if (journalLog.returnDate != null) continue;
                int d = DateTime.Now.Subtract(DateTime.Parse(journalLog.giveDate.ToShortDateString())).Days;
                if (DateTime.Now.Subtract(DateTime.Parse(journalLog.giveDate.ToShortDateString())).Days > context.lateDays)
                        journalLog.isLate = true;
            }
            return context.Journal.ToList();
        }


        public List<JournalLog> GetReaderHistiry() 
        {
            int id;
            Int32.TryParse(Request.Query["id"], out id);
            Console.WriteLine($" getReaderHistory request. id: {id}");
            List <JournalLog> journal = new List<JournalLog>();
            foreach (JournalLog journalLog in context.Journal)
            { 
                if (journalLog.readerId == id)
                    journal.Add(journalLog); 
            }
            return journal;
        }
    }
}
