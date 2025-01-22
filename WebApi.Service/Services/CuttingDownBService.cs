using Bogus;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces.Common;
using WebApi.Domain.Interfaces.Services;

namespace WebApi.Service.Services
{
    public class CuttingDownBService : ICuttingDownBService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CuttingDownBService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> GenerateCableCuttingsAsync()
        {
            if (await _unitOfWork.CuttingDownBRepository.ExistsAsync(x => true))
                return false;

            // Fetch existing data from the database
            var problemTypes = await _unitOfWork.IstaProblemTypeRepository.GetAllAsync();
            var cables = await _unitOfWork.CableRepository.GetAllAsync(); // Get all cables

            var baseCreateDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-30)); // 30 days ago

            var cuttingFaker = new Faker<CuttingDownB>()
                .StrictMode(true)
                .RuleFor(c => c.CuttingDownCableName, f => f.PickRandom(cables).CableName) // Pick from existing cables
                .RuleFor(c => c.ProblemTypeKey, f => f.PickRandom(problemTypes).ProblemTypeKey) // Pick random problem types
                .RuleFor(c => c.CreateDate, f => baseCreateDate.AddDays(f.Random.Int(1, 30))) // Random CreateDate within the past month
                .RuleFor(c => c.EndDate, (f, c) =>
                {
                    // 50% chance of having an EndDate, which will always be after CreateDate and before today
                    if (f.Random.Bool())
                        return c.CreateDate!.Value.AddDays(f.Random.Int(1, 15))
                            .CompareTo(DateOnly.FromDateTime(DateTime.Now)) < 0
                            ? c.CreateDate!.Value.AddDays(f.Random.Int(1, 15))
                            : null;
                    return null;
                })
                .RuleFor(c => c.IsGlobal, f => f.Random.Bool())
                .RuleFor(c => c.IsPlanned, f => f.Random.Bool())
                .RuleFor(c => c.IsActive, (f, c) => c.EndDate != null) // Active only if there’s an EndDate
                .RuleFor(c => c.PlannedStartDts, (f, c) =>
                    c.IsPlanned!.Value ? c.CreateDate!.Value.AddDays(f.Random.Int(-2, 0)) : null)
                .RuleFor(c => c.PlannedEndDts, (f, c) =>
                {
                    // If there's no EndDate and IsPlanned is true, PlannedEndDTS should be after today
                    if (c.IsPlanned!.Value && c.EndDate == null)
                        return DateOnly.FromDateTime(DateTime.Now).AddDays(f.Random.Int(1, 15));
                    return null;
                })
                // Adding rules for missing properties
                .RuleFor(c => c.CreatedUser, f => f.Random.Int(1, 5)) // Random user ID
                .RuleFor(c => c.UpdatedUser, f => f.Random.Int(1, 5)) // Random user ID for update
                // Ignore the identity column (CuttingDownBIncidentId) to prevent validation errors
                .Ignore(c => c.CuttingDownBIncidentId) // Ignore the primary key (identity column)
                .Ignore(c => c.ProblemTypeKeyNavigation); // Ignoring navigation property

            var cuttings = cuttingFaker.Generate(50);

            // Add the generated data to the database
            _unitOfWork.CuttingDownBRepository.AddRange(cuttings);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
