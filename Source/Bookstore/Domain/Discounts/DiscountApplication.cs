using Bookstore.Domain.Common;

namespace Bookstore.Domain.Discounts
{
    public record DiscountApplication(string Label, Money DiscountedAmount);
}
