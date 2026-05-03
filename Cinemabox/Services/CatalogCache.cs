namespace Cinemabox.Services;

public class CatalogCache(IServiceScopeFactory scopeFactory)
{
    private List<VodCategoryResponse>? _vodCategories;
    private List<VodCategoryResponse>? _seriesCategories;

    public async Task<List<VodCategoryResponse>> GetVodCategoriesAsync()
    {
        if (_vodCategories is not null) return _vodCategories;
        using var scope = scopeFactory.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<ApiClient>();
        _vodCategories = await client.GetVodCategoriesAsync();
        return _vodCategories;
    }

    public async Task<List<VodCategoryResponse>> GetSeriesCategoriesAsync()
    {
        if (_seriesCategories is not null) return _seriesCategories;
        using var scope = scopeFactory.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<ApiClient>();
        _seriesCategories = await client.GetSeriesCategoriesAsync();
        return _seriesCategories;
    }

    public void Invalidate()
    {
        _vodCategories = null;
        _seriesCategories = null;
    }
}
