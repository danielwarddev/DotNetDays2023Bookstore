using AutoFixture;
using DotNetDays2023Bookstore.API.Books;
using DotNetDays2023Bookstore.Database;
using FluentAssertions;

namespace DotNetDays2023Bookstore.IntegrationTests;

[Collection(nameof(DatabaseTestCollection))]
public class BookReviewTests : DatabaseHelper
{
    private readonly Fixture _fixture = new();
    private readonly BookService _bookService;
    
    public BookReviewTests(IntegrationTestFactory factory) : base(factory)
    {
        _bookService = new BookService(DbContext);
    }

    [Fact]
    public async Task When_BookReview_Does_Not_Exist_In_Database_Then_Adds_It()
    {
        var expectedReview = _fixture.Create<BookReview>();

        await _bookService.ReviewBook(expectedReview.GutendexBookId, expectedReview.UserId, expectedReview.ReviewContent);

        var allBookReviews = DbContext.BookReviews.ToList();
        allBookReviews.Count.Should().Be(1);
        allBookReviews[0].Should().BeEquivalentTo(expectedReview,
            options => options.Excluding(x => x.Id));
    }

    [Fact]
    public async Task When_BookReview_Exists_In_Database_Already_Then_Updates_Review_Content()
    {
        var existingBookReview = _fixture.Create<BookReview>();
        await AddAsync(existingBookReview);

        var updatedBookReview = existingBookReview;
        updatedBookReview.ReviewContent = "This book is totally cool";

        await _bookService.ReviewBook(updatedBookReview.GutendexBookId, updatedBookReview.UserId, updatedBookReview.ReviewContent);

        var allBookReviews = DbContext.BookReviews.ToList();
        allBookReviews.Count.Should().Be(1);
        allBookReviews[0].Should().BeEquivalentTo(updatedBookReview,
            options => options.Excluding(x => x.Id));
    }
}