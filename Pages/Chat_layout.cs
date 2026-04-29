using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace YourApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
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

            // ✅ Use client WITH headers (not _httpClient)
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");
            client.DefaultRequestHeaders.Add("User-Agent", "MyPortfolioApp");

            var ollamaUrl = Environment.GetEnvironmentVariable("OLLAMA_URL")
                            ?? "https://almost-backtrack-drapery.ngrok-free.dev";

            var response = await client.PostAsJsonAsync(
                $"{ollamaUrl}/api/chat",  // ✅ Using client, not _httpClient
                ollamaRequest
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