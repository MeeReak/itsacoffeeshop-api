using smakchet.application.Interfaces.IVariation;
using smakchet.dal.Models;

namespace smakchet.application.Repositories;

public class VariationRepository(SmakchetContext context) : BaseRepository<Variation>(context), IVariationRepository
{
}