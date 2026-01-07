using pm_backend.Data;
using pm_backend.Services.Commands.Contracts;

namespace pm_backend.Services.Commands
{
    public class UserService : IUserService
    {
        private readonly PmDbContext _context;

        public UserService(PmDbContext context)
        {
            _context = context;
        }

        public async Task UpdateIsDeletedAsync(int id, string isDeleted)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.IsDeleted = isDeleted;

            if (isDeleted == "Y")
            {
                user.IsApproved = "N";
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
