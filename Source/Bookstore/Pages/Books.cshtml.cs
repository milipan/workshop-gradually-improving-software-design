using Bookstore.Data.Seeding;
using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace Bookstore.Pages;

public class BooksModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _dbContext;
    public IEnumerable<Book> Books { get; private set; } = Enumerable.Empty<Book>();
    private readonly IDataSeed<Book> _booksSeed;

    public BooksModel(ILogger<IndexModel> logger, BookstoreDbContext dbContext, IDataSeed<Book> booksSeed)
    {
        _logger = logger;
        _dbContext = dbContext;
        _booksSeed = booksSeed;
    }

    public string Initial { get; set; } = "N/A";
    public async Task OnGet([FromQuery] string? initial = null)
    {
        this.AuthorInitials = await _dbContext.Books.GetBooks()
            .SelectMany(book => book.AuthorsCollection)
            .Select(auth => auth.Person.LastName.Substring(0, 1))
            .Select(auth => auth.ToUpper())
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();

        //this.Initial = initial ?? "N/A";
        await this._booksSeed.SeedAsync();
        this.Books = await _dbContext.Books.GetBooks().ByOptionalAuthorInitial(initial).OrderBy(book => book.Title).ToListAsync();

    }

    private Task<IReadOnlyList<string>> GetAuthotInitials()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> AuthorInitials { get; set; } = Enumerable.Empty<string>();
}
