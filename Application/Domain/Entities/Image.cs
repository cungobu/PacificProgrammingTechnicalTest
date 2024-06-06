using System.Text.Json.Serialization;

namespace Domain.Entities;

public partial class Image
{
    public int Id { get; set; }

    public string? Url { get; set; }
}
