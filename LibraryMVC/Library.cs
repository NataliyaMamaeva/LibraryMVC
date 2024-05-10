using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LibraryMVC
{
    public enum ToDoWithLates
    {
        sms,
        call,
        kill,
        nothing
    }

    public class LibraryContext : DbContext
    {
        public int lateDays;
        public ToDoWithLates lates;
        public LibraryContext()
        {
            Database.EnsureCreated();
            if (Settings.Count() == 0)
                Settings.Add(new SettingsLog());
            SaveChanges();
            lateDays  = Settings.ToArray()[0].days;
            lates = Settings.ToArray()[0].lateReaction;
        }      

        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Reader> Readers { get; set; }
        public virtual DbSet<JournalLog> Journal { get; set; }
        public virtual DbSet<SettingsLog> Settings { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JournalLog>()
             .HasOne(j => j.reader)
             .WithMany()
             .HasForeignKey(j => j.readerId)
             .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<JournalLog>()
            .HasOne(j => j.book)
            .WithMany()
            .HasForeignKey(j => j.bookId)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Book>()
           .HasOne(j => j.author)
           .WithMany()
           .HasForeignKey(j => j.authorId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Book>()
           .HasOne(j => j.reader)
           .WithMany()
           .HasForeignKey(j => j.readerId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Book>()
           .HasOne(j => j.genre)
           .WithMany()
           .HasForeignKey(j => j.genreId)
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Book>()
           .HasOne(j => j.language)
           .WithMany()
           .HasForeignKey(j => j.languageId)
           .OnDelete(DeleteBehavior.NoAction);

            
        }
      
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer("Data Source=THINKPADNATALIY;Initial Catalog=Library;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }

    public class Language
    {
        public int Id { get; set; }
        public string language { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string genre { get; set; }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int languageId { get; set; }
        public Language? language { get; set; }
        public int genreId { get; set; }
        public Genre? genre { get; set; }
        public int authorId { get; set; }
        public Author? author { get; set; }

        public string authorName { get; set; }
        public int year { get; set; }
        public int? readerId { get; set; }
        public Reader? reader { get; set; }
      //  public List<JournalLog>? journal { get; set; }
        public override string ToString()
        {
            return $"{Id}___{Title}___{author?.Name}___{year}";
        }
    }

    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int dateOfBirth { get; set; }
        public string country { get; set; }

    }

    public class Reader
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateOnly BirthDate { get; set; } 
       // public bool HasLateBooks {  get; set; }

        //public List<JournalLog> journal { get; set; }

        public override string ToString()
        {
            return $"{Id}___{Name}___{BirthDate}___{Phone}";
        }
    }

    public class JournalLog
    {
        public int Id { get; set; }
        public string bookTitle { get; set; }
        public string readerName { get; set; }
        public DateOnly giveDate { get; set; }
        public int? readerId { get; set; }
        public Reader? reader { get; set; }
        public int? bookId { get; set; }
        public Book? book { get; set; }
        public DateOnly? returnDate { get; set; }
        public bool isLate {  get; set; }
    }
    public class SettingsLog
    {
        public int Id { get; set; }
        public int days { get; set; } = 14;
        public ToDoWithLates lateReaction { get; set; } = ToDoWithLates.sms;

    }
}
