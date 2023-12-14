namespace Bookstore.Domain.Models
{
    public record DiscountParameters(
        decimal RelativeDiscount,
        string BookTitlePrefix,
        decimal PrefixDiscount);
}
