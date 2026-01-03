using Azure.Core.Pipeline;
using Chibegir.Application.DTOs.AI;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using RestSharp;
using System.Net;
using System.Text;
using System.Text.Json;
using Chibegir.Application.DTOs;
using Microsoft.Extensions.Configuration;
using ZstdSharp.Unsafe;

namespace Chibegir.Infrastructure.Services;

public class ProductExtractorService : IProductExtractorService
{

    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    private readonly RestClient _client;
    private readonly string _apiKey;
    private readonly string _baseUrl;
    private readonly string _model;
    private readonly IConfiguration _configuration;

    public ProductExtractorService(IProductService productService, ICategoryService categoryService, IConfiguration configuration)
    {
        _productService = productService;
        _categoryService = categoryService;
        _configuration = configuration;

        //string apiKey, string proxyUrl = null
        //_apiKey = "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";



        var openAiConfig = configuration.GetSection("OpenAI");
        _apiKey = openAiConfig.GetValue<string>("ApiKey");
        _baseUrl = openAiConfig.GetValue<string>("BaseUrl");
        _model = openAiConfig.GetValue<string>("Model");


        var options = new RestClientOptions(_baseUrl)
        {
            //Timeout = 90_000
        };

        _client = new RestClient(options);


    }
    public async Task<string> ExtractProductWithAiAsync(int productId)
    {

        var product = await _productService.GetProductByIdAsync(productId);
        if (product is null)
            throw new Exception("product not found");

        var attributesForPrompt = await GetAttributesAsync(product.CategoryId);
        var prompt = GeneratePrompt(product, attributesForPrompt, product?.Category?.Name ?? "");

        var result = await CallAsync(prompt);
        return result;
    }




    private async Task<List<string>> GetAttributesAsync(int categoryId)
    {
        var category = await _categoryService.GetCategoryByIdWithAttributesAsync(categoryId);
        var attributes = category?.Attributes;
        var attributesForPrompt = attributes?
            .Select(r => r.Attribute?.AttributeKey)
            .ToList();

        return attributesForPrompt;


    }


    private async Task<string> CallAsync(string finalPrompt)
    {
        var request = new RestRequest("/v1/responses", Method.Post);

        request.AddHeader("Authorization", $"Bearer {_apiKey}");
        request.AddHeader("Content-Type", "application/json");

        var openAiConfiguration = _configuration.GetSection("OpenAI");




        var body = new OpenAIRequestDto
        {
            //"gpt-4.1-mini",
            Model = _model,
            Max_Output_Tokens = 2000,
            Input = new List<OpenAiRequestMessageDto>
            {
                new()
                {
                    Role = "system",
                    Content = "You are an automated product data extraction engine."
                },
                new()
                {
                    Role = "user",
                    Content = finalPrompt
                }
            }
        };

        request.AddJsonBody(body);

        var response = await _client.ExecuteAsync(request);

        if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
            throw new Exception($"OpenAI call failed: {response.StatusCode} - {response.ErrorMessage}");

        var dto = JsonSerializer.Deserialize<OpenAIResponseDto>(
            response.Content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        var outputText = dto?
            .Output?
            .FirstOrDefault()?
            .Content?
            .FirstOrDefault()?
            .Text;

        if (string.IsNullOrWhiteSpace(outputText))
            throw new Exception("OpenAI returned empty output.");

        return outputText; // JSON خام محصول
    }




    private string GeneratePrompt(ProductDto product, List<string> attributes, string category)
    {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("You are an automated product data extraction engine.");
        stringBuilder.AppendLine("You can access and read public product pages via URL or parse provided HTML content.");

        stringBuilder.AppendLine("\r\nStrict rules:" +
                                 "\r\n- If URL is provided, open the page and read the content." +
                                 "\r\n- If HTML content is provided, parse it directly." +
                                 "\r\n- Extract values ONLY for the attributes provided." +
                                 "\r\n- Return category information based on the provided categories list." +
                                 "\r\n- DO NOT create new attributes or categories." +
                                 "\r\n- DO NOT guess values." +
                                 "\r\n- Normalize units when needed (e.g., MB → GB, cm → m)." +
                                 "\r\n- If a value is not found, return null." +
                                 "\r\n- Return numbers as strings." +
                                 "\r\n- Return ONLY valid JSON without explanations, comments, or extra text.\r\n");

        stringBuilder.AppendLine("Product Page URL: ");
        stringBuilder.AppendLine(product.ProductUrl ?? "null");

        stringBuilder.AppendLine("OR Product HTML Content: ");
        stringBuilder.AppendLine(product.Html ?? "null");

        stringBuilder.AppendLine("\r\nAttributes to extract:");
        stringBuilder.AppendLine(System.Text.Json.JsonSerializer.Serialize(attributes));

        stringBuilder.AppendLine("\r\nCategory:");
        stringBuilder.AppendLine(category);

        stringBuilder.AppendLine("\r\nOutput JSON format example:");
        stringBuilder.AppendLine("{");
        foreach (var attr in attributes)
        {
            stringBuilder.AppendLine($"  \"{attr}\": \"\",");
        }
        stringBuilder.AppendLine("  \"category\": \"\"");
        stringBuilder.AppendLine("}");

        return stringBuilder.ToString();
    }


    //[Deprecate]
    private string GeneratePromptAsync(Product product)
    {

        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append("You are an automated product data extraction engine.\r\n");

        stringBuilder.AppendLine("ou can access and read public product pages via URL.");


        stringBuilder.AppendLine("\r\nStrict rules:" +
                                 "\r\n- Open the given URL and read the product page content." +
                                 "\r\n- Extract values ONLY for the attributes provided." +
                                 "\r\n- DO NOT create new attributes." +
                                 "\r\n- DO NOT guess values." +
                                 "\r\n- Normalize units when needed (e.g. MB → GB)." +
                                 "\r\n- If a value is not found, return null." +
                                 "\r\n- Return numbers as strings." +
                                 "\r\n- Return ONLY valid JSON." +
                                 "\r\n- Do NOT add explanations, comments, or extra text.\r\n");

        stringBuilder.AppendLine("Product Page URL: ");
        stringBuilder.AppendLine(product.ProductUrl);



        return stringBuilder.ToString();
    }

}