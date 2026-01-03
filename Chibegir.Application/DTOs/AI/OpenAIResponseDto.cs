namespace Chibegir.Application.DTOs.AI;

public class OpenAIResponseDto
{
    public List<OpenAiOutputDto> Output { get; set; }
}


public class OpenAiOutputDto
{
    public List<OpenAiContentDto> Content { get; set; }
}

public class OpenAiContentDto
{
    public string Type { get; set; }
    public string Text { get; set; }
}