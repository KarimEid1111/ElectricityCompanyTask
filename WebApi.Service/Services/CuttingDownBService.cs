using Bogus;
using WebApi.Domain.Entities;
using WebApi.Domain.Interfaces.Common;
using WebApi.Domain.Interfaces.Services;

namespace WebApi.Service.Services;

public class CuttingDownBService(IUnitOfWork unitOfWork) : ICuttingDownBService
{
    public async Task<bool> GenerateCableCuttingsAsync()
    {
        if (await unitOfWork.CuttingDownBRepository.ExistsAsync(x => true))
            return false;

        var problemTypes = await unitOfWork.IstaProblemTypeRepository.GetAllAsync();
        var cableFirstItem = await unitOfWork.CableRepository.GetAsync(x => true);
        var cableSequenceStart = int.Parse(cableFirstItem.CableName!.Split('-')[1]);

        var baseCreateDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-30)); // CreateDate up to 30 days ago

        var cableNameFaker = new Faker<string>()
            .CustomInstantiator(f =>
            {
                var randomCableIdentifier = f.Random.Int(cableSequenceStart, cableSequenceStart + 700);
                var randomCableQualifier = f.Random.Int(1, 3);
                return $"Cable-{randomCableIdentifier}-{randomCableQualifier}";
            });

        var cableNames = new HashSet<string>();

        var cuttingFaker = new Faker<CuttingDownB>()
            .StrictMode(true)
            .RuleFor(c => c.CuttingDownCableName, f =>
            {
                string name;
                do
                {
                    name = cableNameFaker.Generate();
                } while (!cableNames.Add(name));

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
            .Ignore(c => c.CuttingDownBIncidentId)
            .Ignore(c => c.CreatedUser)
            .Ignore(c => c.UpdatedUser)
            .Ignore(c => c.ProblemTypeKeyNavigation);

        var cuttings = cuttingFaker.Generate(50);

        unitOfWork.CuttingDownBRepository.AddRange(cuttings);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}
