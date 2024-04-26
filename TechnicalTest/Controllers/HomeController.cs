using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetProfileImage([FromQuery] string userIdentifier)
        {
            var url = string.Empty;
            var lastCharacter = GetLastCharacter(userIdentifier);
            switch (lastCharacter) {
                case "6":
                case "7":
                case "8":
                case "9":
                    url = $"https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/{lastCharacter}";
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
                    var vowelMatch = Regex.Match(userIdentifier, "[aeiou]");
                    if (vowelMatch.Success) // at least one vowel character
                    {
                        char firstVowel = vowelMatch.Value[0];
                        url = $"https://api.dicebear.com/8.x/pixel-art/png?seed={firstVowel}&size=150"; // Build url from the first vowel
                    }

                    if (Regex.IsMatch(userIdentifier, "[^a-zA-Z0-9]")) // a non-alphanumeric character
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(1, 6);
                        url = $"https://api.dicebear.com/8.x/pixel-art/png?seed={randomNumber}&size=150";
                    }

                    break;
            }

            if (string.IsNullOrEmpty(url))
            {
                url = $"https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
            }

            return Ok(new { URL = url });
        }

        private string GetLastCharacter(string userIdentifier) {
            if (string.IsNullOrEmpty(userIdentifier))
            {
                return string.Empty;
            }

            return userIdentifier.Last().ToString();
        }
    }
}
