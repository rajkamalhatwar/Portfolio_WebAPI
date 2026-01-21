using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Portfolio_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public AIController(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient();
        }

        [HttpPost("generate-portfolio-content")]
        public async Task<IActionResult> GeneratePortfolioContent([FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Image is required");

            // 1️⃣ Convert image to Base64
            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            var base64Image = Convert.ToBase64String(ms.ToArray());

            // 2️⃣ AI Prompt
            var prompt = @"
Analyze the image and generate:
1. A short professional portfolio title (max 8 words)
2. A creative description (2–3 lines)
3. 5 relevant tags
4. Category (Sketches / Photography / Logo Design / Poster Design)

Rules:
- Do not mention AI
- Keep it artistic and professional
- Output JSON only in this format:

{
  ""title"": """",
  ""description"": """",
  ""tags"": [],
  ""category"": """"
}";

            // 3️⃣ OpenAI Request Body
            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new object[]
                {
                new { role = "user", content = prompt },
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new
                        {
                            type = "input_image",
                            image_base64 = base64Image
                        }
                    }
                }
                }
            };

            // 4️⃣ HTTP Request
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", _config["OpenAI:ApiKey"]);

            request.Content = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            // 5️⃣ Send request
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // 6️⃣ Extract AI JSON
            var aiText = JObject.Parse(responseString)
                ["choices"]?[0]?["message"]?["content"]?.ToString();

            var cleanJson = JsonConvert.DeserializeObject(aiText);

            // 7️⃣ Return clean JSON to React
            return Ok(cleanJson);
        }
         }
}
