using DotNetDays2023Bookstore.API.Books;

namespace DotNetDays2023Bookstore.API;

public interface ILotteryService
{
    Task<bool> CheckIfUserWon(IEnumerable<Book> booksPurchased);
}

public class LotteryService : ILotteryService
{
    private readonly IBookClient _bookClient;

    public LotteryService(IBookClient bookClient)
    {
        _bookClient = bookClient;
    }

    public async Task<bool> CheckIfUserWon(IEnumerable<Book> booksPurchased)
    {
        var lotteryNumber = await _bookClient.ProcessPurchasedBooks(booksPurchased.Where(x => x.Copyright));
        return lotteryNumber == 123456789;
    }
}