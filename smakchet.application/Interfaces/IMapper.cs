namespace smakchet.application.Interfaces
{
    public interface IMapper<TEntity, TReadDto, TCreateDto, TUpdateDto>
    {
        TReadDto ToReadDto(TEntity entity);
        TEntity ToEntity(TCreateDto dto);
        void UpdateEntity(TEntity entity, TUpdateDto dto);
    }
}
