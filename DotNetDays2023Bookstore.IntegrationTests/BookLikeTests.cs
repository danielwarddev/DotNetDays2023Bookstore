using AutoFixture;
using DotNetDays2023Bookstore.API.Books;
using DotNetDays2023Bookstore.Database;
using FluentAssertions;

namespace DotNetDays2023Bookstore.IntegrationTests;

[Collection(nameof(DatabaseTestCollection))]
public class BookLikeTests : DatabaseHelper
{
    private readonly Fixture _fixture = new();
    private readonly BookService _bookService;

    public BookLikeTests(IntegrationTestFactory factory) : base(factory)
    {
        _bookService = new BookService(DbContext);
    }

    [Fact]
    public async Task When_BookLike_Does_Not_Exist_In_Database_Then_Adds_It()
    {
        var expectedLike = _fixture.Create<BookLike>();
        
        await _bookService.LikeBook(expectedLike.GutendexBookId, expectedLike.UserId);

        var allBookLikes = DbContext.BookLikes.ToList();
        allBookLikes.Count.Should().Be(1);
        allBookLikes[0].Should().BeEquivalentTo(expectedLike,
            options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task When_BookLike_Exists_In_Database_Already_Then_Does_Nothing()
    {
        var gutendexId = _fixture.Create<int>();
        var userId = _fixture.Create<int>();

        await AddAsync(new BookLike()
        {
            GutendexBookId = gutendexId,
            UserId = userId
        });

        await _bookService.LikeBook(gutendexId, userId);

        var allBookLikes = DbContext.BookLikes.ToList();
        allBookLikes.Count.Should().Be(1);
    }
}