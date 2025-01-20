namespace WebApi.Domain.Interfaces.Services;

public interface ICuttingDownAService
{
    Task<bool> GenerateCabinCuttingsAsync();
}