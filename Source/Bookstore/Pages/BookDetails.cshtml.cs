using Bookstore.Domain.Common;
using Bookstore.Domain.Discounts;
using Bookstore.Domain.Models;
using Bookstore.Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookstore.Pages;

public class BookDetailsModel : PageModel
{
    public record PriceLine(string Label, Money Amount);

    private readonly ILogger<IndexModel> _logger;
    private readonly BookstoreDbContext _dbContext;
    public Book Book { get; private set; } = null!;

    private readonly IDiscount _discounts;

    public IReadOnlyList<PriceLine> PriceSpecification { get; private set; } = Array.Empty<PriceLine>();

    public BookDetailsModel(ILogger<IndexModel> logger, BookstoreDbContext dbContext,
                            IDiscount discounts) =>
        (_logger, _dbContext, _discounts) = (logger, dbContext, discounts);

    public async Task<IActionResult> OnGet(Guid id)
    {
        if ((await _dbContext.Books.GetBooks().ById(id)) is Book book)
        {
            this.Book = book;
            Money price = BookPricing.SeedPriceFor(book, Currency.USD).Value;

            var priceModifications = _discounts.ApplyTo(book, price).GetEnumerator();

            var priceSpec = new List<PriceLine>();

            if (priceModifications.MoveNext())
            {
                priceSpec.Add(new("Original price", price));
                var finalPrice = price;
                do
                {
                    var modification = priceModifications.Current;
                    finalPrice = finalPrice - modification.DiscountedAmount;
                    priceSpec.Add(new(modification.Label, modification.DiscountedAmount));
                }
                while (priceModifications.MoveNext());

                priceSpec.Add(new("Total", finalPrice));
            }
            else
            {
                priceSpec.Add(new("Price", price));
            }

            this.PriceSpecification = priceSpec;

            return Page();
        }

        return Redirect("/books");
    }
}