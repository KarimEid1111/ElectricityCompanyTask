namespace WebApi.Domain.Interfaces.Services;

public interface ICuttingDownBService
{
    Task<bool> GenerateCableCuttingsAsync();
}