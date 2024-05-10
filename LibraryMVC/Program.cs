using LibraryMVC;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


//LibraryContext context = new LibraryContext();

//List<Genre> genres = new() { new Genre() { genre = "драма" }, new Genre() { genre = "сказка" }, new Genre() { genre = "детектив" }, new Genre() { genre = "учебник" } };
//context.Genres.AddRange(genres);
//context.SaveChanges();

//List<Language> languages = new() { new Language() { language = "русский" }, new Language() { language = "английский" }, new Language() { language = "французский" } };
//context.Languages.AddRange(languages);
//context.SaveChanges();

//List<Reader> readers = new() { new Reader() { Name = "Василий", BirthDate = new DateOnly(2001,04,30), Phone = "8 999 999 99 99" },
//                                new Reader() { Name = "Пётр Иванович Петров", BirthDate = new DateOnly(1967,12,12), Phone = "8 888 888 88 88" },
//                                new Reader() { Name = "Анна Блохина", BirthDate = new DateOnly(1991,01,30), Phone = "7 777 777 77 77" },
//                                new Reader() { Name = "Авраам Линкольн", BirthDate = new DateOnly(2006,04,30), Phone = "7 999 666 55 44" },
//                                new Reader() { Name = "Спандж Боб", BirthDate = new DateOnly(1995,11,13), Phone = "8 678 654 33 22" }};

//context.Readers.AddRange(readers);
//context.SaveChanges();

//List<Author> authors = new() { new Author() { Name = "Александр Сергеевич Пушкин", country = "Российская Империя", dateOfBirth = 1799 },
//new Author() { Name = "Виктор Гюго", country = "Франция", dateOfBirth = 1802 },
//new Author() { Name = "Борис Акунин", country = "СССР", dateOfBirth = 1956 },
//new Author() { Name = "Вильям Шекспир", country = "Великобритания", dateOfBirth = 1564 }};
//context.Authors.AddRange(authors);
//context.SaveChanges();

//List<Book> books = new() { new Book() { Title = "Евгений Онегин", authorName = "Александр Сергеевич Пушкин", authorId = 1, genreId = 1, languageId = 1, year = 1823 },
//new Book() { Title = "Капитанская дочка", authorName = "Александр Сергеевич Пушкин", authorId = 1, genreId = 1, languageId = 1, year = 1836 },
//new Book() { Title = "Пророк", authorName = "Александр Сергеевич Пушкин", authorId = 1, genreId = 1, languageId = 1, year = 1828 },
//new Book() { Title = "Медный всадник", authorName = "Александр Сергеевич Пушкин", authorId = 1, genreId = 1, languageId = 1, year = 1837 },
//new Book() { Title = "Гамлет", authorName = "Вильям Шекспир", authorId = 1, genreId = 4, languageId = 2, year = 1623 },
//new Book() { Title = "Король лир", authorName = "Вильям Шекспир", authorId = 4, genreId = 1, languageId = 2, year = 1606 },
//new Book() { Title = "Отверженные", authorName = "Виктор Гюго", authorId = 2, genreId = 1, languageId = 3, year = 1862 },
//new Book() { Title = "Созерцания", authorName = "Виктор Гюго", authorId = 2, genreId = 1, languageId = 3, year = 1856 },
//new Book() { Title = "Азазель", authorName = "Борис Акунин", authorId = 3, genreId = 3, languageId = 1, year = 1998 },
//new Book() { Title = "История Российского Государства", authorName = "Борис Акунин", authorId = 3, genreId = 4, languageId = 1, year = 2012 }};
//context.Books.AddRange(books);
//context.SaveChanges();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Library}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Reader}/{action=PutReader}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Reader}/{action=PostReader}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Reader}/{action=GetReaderById}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Reader}/{action=GetReaderByName}/{name?}");

app.Run();
