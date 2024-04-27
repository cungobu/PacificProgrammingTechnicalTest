using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TechnicalTest.Models;

public partial class Image
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = null!;
}
