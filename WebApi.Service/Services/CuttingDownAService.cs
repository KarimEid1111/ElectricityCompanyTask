using Bogus;
using WebApi.Domain.Context;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces.Common;
using WebApi.Domain.Interfaces.Services;

namespace WebApi.Service.Services;

public class CuttingDownAService(IUnitOfWork unitOfWork) : ICuttingDownAService
{
    public async Task<bool> GenerateCabinCuttingsAsync()
    {
        if (await unitOfWork.CuttingDownARepository.ExistsAsync(x => true))
            return false;

        var problemTypes = await unitOfWork.IstaProblemTypeRepository.GetAllAsync();
        var cabinFirstItem = await unitOfWork.CabinRepository.GetAsync(x => true);
        var cabinSequenceStart = int.Parse(cabinFirstItem.CabinName!.Split('-')[1]);

        var baseCreateDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-365)); // CreateDate is up to 365 days ago

        var cabinNameFaker = new Faker<string>()
            .CustomInstantiator(f =>
            {
                var randomCabinIdentifier = f.Random.Int(cabinSequenceStart, cabinSequenceStart + 700);
                var randomCabinQualifier = f.Random.Int(1, 3);
                return $"Cabin-{randomCabinIdentifier}-{randomCabinQualifier}";
            });

        var cabinNames = new HashSet<string>();

        var cuttingFaker = new Faker<CuttingDownA>()
            .StrictMode(true)
            .RuleFor(c => c.CuttingDownCabinName, f =>
            {
                string name;
                do
                {
                    name = cabinNameFaker.Generate();
                } while (!cabinNames.Add(name));

                return name;
            })
            .RuleFor(c => c.CreateDate, f => baseCreateDate.AddDays(f.Random.Int(1, 30))) // CreateDate is before today
            .RuleFor(c => c.EndDate, (f, c) =>
            {
                // 50% chance of having an EndDate, which will always be after CreateDate and before today
                if (f.Random.Bool()) 
                    return c.CreateDate!.Value.AddDays(f.Random.Int(1, 15)).CompareTo(DateOnly.FromDateTime(DateTime.Now)) < 0
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
            .RuleFor(c => c.ProblemTypeKey, f => f.PickRandom(problemTypes).ProblemTypeKey)
            .Ignore(c => c.CuttingDownAIncidentId)
            .Ignore(c => c.CreatedUser)
            .Ignore(c => c.UpdatedUser)
            .Ignore(c => c.ProblemTypeKeyNavigation);

        var cuttings = cuttingFaker.Generate(50);

        unitOfWork.CuttingDownARepository.AddRange(cuttings);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}
