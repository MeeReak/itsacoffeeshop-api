using smakchet.application.DTOs.LookUp;

namespace smakchet.application.Interfaces.ILookupService
{
    public interface ILookupService
    {
        Task<LookupReadDto> GetLookupAsync();
    }
}