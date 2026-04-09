using Microsoft.Extensions.Caching.Memory;
using smakchet.application.DTOs.CoffeeLevel;
using smakchet.application.DTOs.Ice;
using smakchet.application.DTOs.LookUp;
using smakchet.application.DTOs.Size;
using smakchet.application.DTOs.Sugar;
using smakchet.application.DTOs.Variation;
using smakchet.application.Interfaces.ICoffeeLevel;
using smakchet.application.Interfaces.IIce;
using smakchet.application.Interfaces.ILookupService;
using smakchet.application.Interfaces.ISize;
using smakchet.application.Interfaces.ISugar;
using smakchet.application.Interfaces.IVariation;

namespace smakchet.application.Services
{
public class LookupService(
    ISizeRepository sizeRepository,
    ISugarRepository sugarRepository,
    IIceRepository iceRepository,
    ICoffeeLevelRepository coffeeLevelRepository,
    IVariationRepository variationRepository,
    IMemoryCache cache
) : ILookupService
    {
    public async Task<LookupReadDto> GetLookupAsync()
    {
      const string cacheKey = "lookup_all";

      if (!cache.TryGetValue(cacheKey, out LookupReadDto? cachedLookup))
      {
        var sizes = await sizeRepository.GetAllAsync(CancellationToken.None);
        var sugars = await sugarRepository.GetAllAsync(CancellationToken.None);
        var ices = await iceRepository.GetAllAsync(CancellationToken.None);
        var coffeeLevels = await coffeeLevelRepository.GetAllAsync(CancellationToken.None);
        var variations = await variationRepository.GetAllAsync(CancellationToken.None);

        cachedLookup = new LookupReadDto
        {
          Sizes = [.. sizes.Select(x => new SizeReadDto
          {
            Id = x.Id,
            Name = x.Name,
            CreatedAt = x.CreatedAt,
          })],

          Sugars = [.. sugars.Select(x => new SugarReadDto
          {
            Id = x.Id,
            Name = x.Name,
            CreatedAt = x.CreatedAt
          })],

          Ices = [.. ices.Select(x => new IceReadDto
          {
            Id = x.Id,
            Name = x.Name,
            CreatedAt = x.CreatedAt
          })],

          CoffeeLevels = [.. coffeeLevels.Select(x => new CoffeeLevelReadDto
          {
            Id = x.Id,
            Name = x.Name,
            CreatedAt = x.CreatedAt
          })],

          Variations = [.. variations.Select(x => new VariationReadDto
          {
            Id = x.Id,
            Name = x.Name,
            CreatedAt = x.CreatedAt
          })]
        };

        // Cache for 12 hours
        cache.Set(cacheKey, cachedLookup, TimeSpan.FromHours(12));
      }

      return cachedLookup!;
    }
  }
}
