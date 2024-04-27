using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;
using TechnicalTest.Models;

namespace TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public HomeController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [EnableCors("AllowAnyOrigin")]
        [HttpGet("/avatar")]
        public async Task<IActionResult> GetProfileImage([FromQuery] string? userIdentifier)
        {
            var url = $"https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
            if (string.IsNullOrEmpty(userIdentifier))
            {
                return Ok(new { URL = url });
            }

            var lastCharacter = userIdentifier.Last().ToString();
            switch (lastCharacter)
            {
                case "6":
                case "7":
                case "8":
                case "9":
                    var myJsonUrl = $"https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/{lastCharacter}";
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(myJsonUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            string json = await response.Content.ReadAsStringAsync();
                            var data = JsonSerializer.Deserialize<Image>(json);
                            if (data != null)
                            {
                                url = data.Url;
                            }
                        }
                    }

                    break;

                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                    var lastDigit = int.Parse(lastCharacter);
                    var image = await _dataContext.Images.FirstOrDefaultAsync(img => img.Id == lastDigit);
                    if (image != null)
                    {
                        url = image.Url;
                    }

                    break;

                default:
                    if (Regex.IsMatch(userIdentifier, "[aeiou]")) // at least one vowel character
                    {
                        url = $"https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150";
                    }
                    else if (Regex.IsMatch(userIdentifier, "[^a-zA-Z0-9]")) // a non-alphanumeric character
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(1, 6);
                        url = $"https://api.dicebear.com/8.x/pixel-art/png?seed={randomNumber}&size=150";
                    }

                    break;
            }

            return Ok(new { URL = url });
        }
    }
}
