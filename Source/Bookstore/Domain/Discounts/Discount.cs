using Bookstore.Domain.Common;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Discounts;

public class RelativeDiscount : IDiscount
{
    public IEnumerable<DiscountApplication> ApplyTo(Book book, Money price)
    {
        throw new NotImplementedException();
    }
}

public class PrefixDiscount : IDiscount
{
    public IEnumerable<DiscountApplication> ApplyTo(Book book, Money price)
    {
        throw new NotImplementedException();
    }
}

public class Discount : IDiscount
{
    private readonly DiscountParameters _discountParameters;

    public Discount(DiscountParameters discountParameters)
    {
        _discountParameters = discountParameters;
    }

    public IEnumerable<DiscountApplication> ApplyTo(Book book, Money price)
    {
        var total = price;
        if (_discountParameters.RelativeDiscount > 0)
        {
            var discount = total * _discountParameters.RelativeDiscount;
            total = total - discount;
            yield return new("Discount", discount);
        }

        bool prefixDiscountApplies =
            !string.IsNullOrWhiteSpace(_discountParameters.BookTitlePrefix) &&
            book.Title.StartsWith(_discountParameters.BookTitlePrefix, StringComparison.OrdinalIgnoreCase) &&
             _discountParameters.PrefixDiscount > 0;

        if (prefixDiscountApplies)
        {
            var discount = total * _discountParameters.PrefixDiscount;
            total = total - discount;
            yield return new($"Discount on books starting with {_discountParameters.BookTitlePrefix}", discount);
        }
    }
}
