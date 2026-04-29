using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace YourApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]   // 🔥 CHANGE THIS
    public class ChatController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ChatController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [HttpPost("GetResponse")]
        public async Task<IActionResult> GetResponse([FromBody] ChatRequest request)
        {
            var ollamaRequest = new
            {
                model = "minimax-m2.7:cloud",
                messages = new[]
                {
                    new { role = "user", content = request.Message }
                },
                stream = false
            };
            
            var client = new HttpClient();
client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");
client.DefaultRequestHeaders.Add("User-Agent", "MyPortfolioApp");

var response = await client.PostAsJsonAsync(
    "https://almost-backtrack-drapery.ngrok-free.dev/api/chat",
    requestBody
);

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OllamaResponse>(json);

            string reply = result?.message?.content ?? "No response";

            return Ok(new { reply });
        }
    }

    public class ChatRequest
    {
        public string? Message { get; set; }
    }

    public class OllamaResponse
    {
        public ChatMessage? message { get; set; }
    }

    public class ChatMessage
    {
        public string? role { get; set; }
        public string? content { get; set; }
    }
}