using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetDays2023Bookstore.Database;

public class BookReview
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int GutendexBookId { get; set; }
    public int UserId { get; set; }
    public string ReviewContent { get; set; }
}