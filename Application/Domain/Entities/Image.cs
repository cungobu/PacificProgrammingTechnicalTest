using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public partial class Image
{
    [Key]
    public int Id { get; set; }

    public string? Url { get; set; }
}
