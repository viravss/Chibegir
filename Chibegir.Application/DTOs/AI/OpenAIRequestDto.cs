using System.Text.Json.Serialization;

namespace Chibegir.Application.DTOs.AI;

public class OpenAIRequestDto
{
    [JsonPropertyName("model")]
    public string Model { get; set; }
    [JsonPropertyName("input")]
    public List<OpenAiRequestMessageDto> Input { get; set; }
    [JsonPropertyName("max_output_tokens")]
    public int Max_Output_Tokens { get; set; }
}

public class OpenAiRequestMessageDto
{
    public string Role { get; set; }
    public string Content { get; set; }
}