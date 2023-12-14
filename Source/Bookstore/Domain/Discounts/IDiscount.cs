using Bookstore.Domain.Common;
using Bookstore.Domain.Models;

namespace Bookstore.Domain.Discounts;

public interface IDiscount
{
    IEnumerable<DiscountApplication> ApplyTo(Book book, Money price);
}
