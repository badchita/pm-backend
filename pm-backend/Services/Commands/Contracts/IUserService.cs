using pm_backend.DTOs;
using pm_backend.Models;

namespace pm_backend.Services.Commands.Contracts
{
    public interface IUserService
    {
        Task UpdateIsDeletedAsync(int id, string isDeleted);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> UpdateUserAsync(int id, UserDto request);
    }
}
