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

//List<Genre> genres = new() { new Genre() { genre = "�����" }, new Genre() { genre = "������" }, new Genre() { genre = "��������" }, new Genre() { genre = "�������" } };
//context.Genres.AddRange(genres);
//context.SaveChanges();

//List<Language> languages = new() { new Language() { language = "�������" }, new Language() { language = "����������" }, new Language() { language = "�����������" } };
//context.Languages.AddRange(languages);
//context.SaveChanges();

//List<Reader> readers = new() { new Reader() { Name = "�������", BirthDate = new DateOnly(2001,04,30), Phone = "8 999 999 99 99" },
//                                new Reader() { Name = "ϸ�� �������� ������", BirthDate = new DateOnly(1967,12,12), Phone = "8 888 888 88 88" },
//                                new Reader() { Name = "���� �������", BirthDate = new DateOnly(1991,01,30), Phone = "7 777 777 77 77" },
//                                new Reader() { Name = "������ ��������", BirthDate = new DateOnly(2006,04,30), Phone = "7 999 666 55 44" },
//                                new Reader() { Name = "������ ���", BirthDate = new DateOnly(1995,11,13), Phone = "8 678 654 33 22" }};

//context.Readers.AddRange(readers);
//context.SaveChanges();

//List<Author> authors = new() { new Author() { Name = "��������� ��������� ������", country = "���������� �������", dateOfBirth = 1799 },
//new Author() { Name = "������ ����", country = "�������", dateOfBirth = 1802 },
//new Author() { Name = "����� ������", country = "����", dateOfBirth = 1956 },
//new Author() { Name = "������ �������", country = "��������������", dateOfBirth = 1564 }};
//context.Authors.AddRange(authors);
//context.SaveChanges();

//List<Book> books = new() { new Book() { Title = "������� ������", authorName = "��������� ��������� ������", authorId = 1, genreId = 1, languageId = 1, year = 1823 },
//new Book() { Title = "����������� �����", authorName = "��������� ��������� ������", authorId = 1, genreId = 1, languageId = 1, year = 1836 },
//new Book() { Title = "������", authorName = "��������� ��������� ������", authorId = 1, genreId = 1, languageId = 1, year = 1828 },
//new Book() { Title = "������ �������", authorName = "��������� ��������� ������", authorId = 1, genreId = 1, languageId = 1, year = 1837 },
//new Book() { Title = "������", authorName = "������ �������", authorId = 1, genreId = 4, languageId = 2, year = 1623 },
//new Book() { Title = "������ ���", authorName = "������ �������", authorId = 4, genreId = 1, languageId = 2, year = 1606 },
//new Book() { Title = "�����������", authorName = "������ ����", authorId = 2, genreId = 1, languageId = 3, year = 1862 },
//new Book() { Title = "����������", authorName = "������ ����", authorId = 2, genreId = 1, languageId = 3, year = 1856 },
//new Book() { Title = "�������", authorName = "����� ������", authorId = 3, genreId = 3, languageId = 1, year = 1998 },
//new Book() { Title = "������� ����������� �����������", authorName = "����� ������", authorId = 3, genreId = 4, languageId = 1, year = 2012 }};
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
