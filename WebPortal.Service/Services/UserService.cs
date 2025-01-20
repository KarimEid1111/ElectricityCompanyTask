using WebPortalDomain.Entities;
using WebPortalDomain.Interfaces.Common;
using WebPortalDomain.Interfaces.Services;

namespace WebPortal.Service.Services;

public class UserService(IUnitOfWork unitOfWork) : IUserService
{
    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        return await unitOfWork.UserRepository.GetSingleAsync(u => u.Name == username && u.Password == password);
    }
}