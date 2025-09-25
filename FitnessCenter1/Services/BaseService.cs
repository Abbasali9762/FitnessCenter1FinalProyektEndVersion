using FitnessCenter1.Context;

namespace FitnessCenter1.Services
{
    public abstract class BaseService
    {
        protected readonly FitnessCenterDbContext _context;

        protected BaseService(FitnessCenterDbContext context)
        {
            _context = context;
        }
    }
}