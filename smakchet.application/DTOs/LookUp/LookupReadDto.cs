using smakchet.application.DTOs.CoffeeLevel;
using smakchet.application.DTOs.Ice;
using smakchet.application.DTOs.Size;
using smakchet.application.DTOs.Sugar;
using smakchet.application.DTOs.Variation;

namespace smakchet.application.DTOs.LookUp
{
    public class LookupReadDto
    {
        public List<IceReadDto>? Ices { get; set; }
        public List<SizeReadDto>? Sizes { get; set; }
        public List<SugarReadDto>? Sugars { get; set; }
        public List<CoffeeLevelReadDto>? CoffeeLevels { get; set; }
        public List<VariationReadDto>? Variations { get; set; }
    }
}