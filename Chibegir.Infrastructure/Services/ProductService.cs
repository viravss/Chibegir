using Chibegir.Application.DTOs;
using Chibegir.Application.Interfaces;
using Chibegir.Domain.Entities;
using Microsoft.Playwright;
using System.Net;
using System.Text.RegularExpressions;

namespace Chibegir.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductSourceRepository _productSourceRepository;
    private readonly IProductLogRepository _productLogRepository;
    private readonly HttpClient _httpClient;
    public ProductService(IProductRepository productRepository, IProductSourceRepository productSourceRepository, IProductLogRepository productLogRepository, HttpClient httpClient)
    {
        _productRepository = productRepository;
        _productSourceRepository = productSourceRepository;
        _productLogRepository = productLogRepository;
        _httpClient = httpClient;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdWithSourceAsync(id, cancellationToken);
        return product == null ? null : await MapToDtoWithSourcesAsync(product, cancellationToken);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllWithSourcesAsync(cancellationToken);
        return products.Select(MapToDto);
    }

    //public async Task<IEnumerable<ProductDto>> GetProductsBySourceIdAsync(int sourceId, CancellationToken cancellationToken = default)
    //{
    //    var products = await _productRepository.FindAsync(p => p.SourceId == sourceId, cancellationToken);
    //    return products.Select(MapToDto);
    //}

    public async Task<IEnumerable<ProductDto>> GetAvailableProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAvailableWithSourcesAsync(cancellationToken);
        return products.Select(MapToDto);
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var product = MapToEntity(productDto);
        product = await _productRepository.AddAsync(product, cancellationToken);

        // Create ProductSource record if SourceId is provided
        if (productDto.SourceId.HasValue && productDto.SourceId.Value > 0)
        {
            var productSource = new ProductSource
            {
                ProductId = product.Id,
                SourceId = productDto.SourceId.Value
            };
            await _productSourceRepository.AddAsync(productSource, cancellationToken);
        }

        // Create ProductLog for Insert action
        var productLog = new ProductLog
        {
            ProductId = product.Id,
            SourceId = productDto.SourceId,
            Action = "Insert"
        };
        await _productLogRepository.AddAsync(productLog, cancellationToken);

        return MapToDto(product);
    }


    public async Task<ProductDto> CreateProductWithHtmlAsync(ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var html = await GetHtmlFromUrlAsync(productDto.ProductUrl);
        productDto.Html = await CleanMyHtml(html);
        var product = await CreateProductAsync(productDto, cancellationToken);
        return product;
    }


    public async Task<ProductDto> UpdateProductAsync(int id, ProductDto productDto, CancellationToken cancellationToken = default)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (existingProduct == null)
            throw new KeyNotFoundException($"Product with id {id} not found.");

        existingProduct.Title = productDto.Title;
        existingProduct.LastUpdate = productDto.LastUpdate;
        existingProduct.ProductUrl = productDto.ProductUrl;
        existingProduct.Html = productDto.Html;
        existingProduct.Price = productDto.Price;
        existingProduct.IsAvailable = productDto.IsAvailable;
        existingProduct.CategoryId = productDto.CategoryId;
        existingProduct.ModifiedOn = DateTime.UtcNow;

        await _productRepository.UpdateAsync(existingProduct, cancellationToken);

        // Get SourceId from ProductSource or use from productDto
        int? sourceId = productDto.SourceId;
        if (!sourceId.HasValue)
        {
            var productSources = await _productSourceRepository.GetByProductIdWithRelationsAsync(id, cancellationToken);
            sourceId = productSources.FirstOrDefault()?.SourceId;
        }

        // Create ProductLog for Update action
        var productLog = new ProductLog
        {
            ProductId = existingProduct.Id,
            SourceId = sourceId,
            Action = "Update"
        };
        await _productLogRepository.AddAsync(productLog, cancellationToken);

        return MapToDto(existingProduct);
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (product == null)
            return false;

        await _productRepository.DeleteAsync(product, cancellationToken);
        return true;
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Title = product.Title,
            LastUpdate = product.LastUpdate,
            ProductUrl = product.ProductUrl,
            Html = product.Html,
            Price = product.Price,
            IsAvailable = product.IsAvailable,
            CategoryId = product.CategoryId,
            Category = product.Category != null ? new CategoryDto
            {
                Id = product.Category.Id,
                Name = product.Category.Name,
                Description = product.Category.Description,
                IsActive = product.Category.IsActive,
                CreatedOn = product.Category.CreatedOn,
                ModifiedOn = product.Category.ModifiedOn
            } : null,
            CreatedOn = product.CreatedOn,
            ModifiedOn = product.ModifiedOn
        };
    }

    private async Task<ProductDto> MapToDtoWithSourcesAsync(Product product, CancellationToken cancellationToken = default)
    {
        var dto = MapToDto(product);

        // Get ProductSources with Sources for this product
        var productSources = await _productSourceRepository.GetByProductIdWithRelationsAsync(product.Id, cancellationToken);
        if (productSources.Any())
        {
            dto.Sources = productSources
                .Where(ps => ps.Source != null)
                .Select(ps => new SourceDto
                {
                    Id = ps.Source!.Id,
                    SourceName = ps.Source.SourceName,
                    SourceBaseAddress = ps.Source.SourceBaseAddress,
                    IsActive = ps.Source.IsActive,
                    CreatedOn = ps.Source.CreatedOn,
                    ModifiedOn = ps.Source.ModifiedOn
                })
                .ToList();

            // Set SourceId from first source if available
            dto.SourceId = dto.Sources.FirstOrDefault()?.Id;
        }

        return dto;
    }

    private static Product MapToEntity(ProductDto productDto)
    {
        return new Product
        {
            Id = productDto.Id,
            Title = productDto.Title,
            LastUpdate = productDto.LastUpdate,
            ProductUrl = productDto.ProductUrl,
            Html = productDto.Html,
            Price = productDto.Price,
            IsAvailable = productDto.IsAvailable,
            CategoryId = productDto.CategoryId
        };
    }

    private async Task<string> GetHtmlFromUrlAsync(string url)
    {
        var html = await GetHtmlByHttpClient(url);
        if (html.Contains("application/ld+json"))
        {
            return html;
        }
        html = await GetHtmlByPlayWrite(url);

        return html;
    }


    private async Task<string> GetHtmlByHttpClient(string url)
    {
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64)"
        );

        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("fa-IR,fa;q=0.9");

        var html = await _httpClient.GetStringAsync(new Uri(url));
        return html;
    }
    private async Task<string> GetHtmlByPlayWrite(string url)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        var page = await browser.NewPageAsync();
        await page.GotoAsync(url, new PageGotoOptions
        {
            WaitUntil = WaitUntilState.NetworkIdle,

        });

        await page.WaitForTimeoutAsync(3000);

        // HTML
        return await page.ContentAsync();
    }

    //private async Task<string> CleanMyHtml(string html)
    //{
    //    html = Regex.Replace(html, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    //    html = Regex.Replace(html, "<style.*?</style>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    //    html = Regex.Replace(html, "<link.*?rel=[\"']stylesheet[\"'].*?>", "", RegexOptions.IgnoreCase);
    //    html = Regex.Replace(html, "<symbol.*?</symbol>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    //    html = Regex.Replace(html, "<svg.*?</svg>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

    //    return html;
    //}

    private async Task<string> CleanMyHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
            return string.Empty;

        html = Regex.Replace(html, "<!--.*?-->", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<script.*?</script>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<style.*?</style>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<link.*?rel=[\"']stylesheet[\"'].*?>", "", RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<symbol.*?</symbol>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<svg.*?</svg>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<iframe.*?</iframe>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<noscript.*?</noscript>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<meta.*?>", "", RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "data:image/.*?;base64,[^\"]*", "", RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<img.*?>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        html = Regex.Replace(html, @"\s{2,}", " ");
        html = Regex.Replace(html, @"\n{2,}", "\n");

        html = Regex.Replace(html, "<head.*?</head>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<link.*?>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        html = Regex.Replace(html, "\\s*style\\s*=\\s*\"[^\"]*\"", "", RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<picture.*?</picture>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        html = Regex.Replace(html, "<footer.*?</footer>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);



        return html.Trim();
    }






}