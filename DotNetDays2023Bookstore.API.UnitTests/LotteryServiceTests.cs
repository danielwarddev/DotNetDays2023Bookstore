
using AutoFixture;
using DotNetDays2023Bookstore.API.Books;
using FluentAssertions;
using NSubstitute;

namespace DotNetDays2023Bookstore.API.UnitTests;

public class LotteryServiceTests
{
    private readonly LotteryService _lotteryService;
    private readonly IBookClient _bookClient = Substitute.For<IBookClient>();
    private readonly Fixture _fixture = new();

    public LotteryServiceTests()
    {
        _lotteryService = new LotteryService(_bookClient);
    }
    
    [Fact]
    public async Task When_Lottery_Number_Is_123456789_Then_Returns_True()
    {
        var copyrightedBooks = _fixture.Build<Book>()
            .With(x => x.Copyright, true)
            .CreateMany();
        var nonCopyrightedBook = _fixture.Build<Book>()
            .With(x => x.Copyright, false)
            .Create();
        var allBooks = new List<Book>(copyrightedBooks) { nonCopyrightedBook };

        _bookClient.ProcessPurchasedBooks(Arg.Any<IEnumerable<Book>>()).Returns(123456789);
        
        var userWon = await _lotteryService.CheckIfUserWon(allBooks);

        userWon.Should().BeTrue();
    }
    
    [Fact]
    public async Task When_Lottery_Number_Is_Not_123456789_Then_Returns_False()
    {
        var copyrightedBooks = _fixture.Build<Book>()
            .With(x => x.Copyright, true)
            .CreateMany();
        var nonCopyrightedBook = _fixture.Build<Book>()
            .With(x => x.Copyright, false)
            .Create();
        var allBooks = new List<Book>(copyrightedBooks) { nonCopyrightedBook };

        _bookClient.ProcessPurchasedBooks(ArgIs.EquivalentTo(copyrightedBooks)).Returns(1);

        var userWon = await _lotteryService.CheckIfUserWon(allBooks);

        userWon.Should().BeFalse();
    }
}
