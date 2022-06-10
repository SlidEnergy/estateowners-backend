using EstateOwners.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EstateOwners.App
{
    public class UsersService : IUsersService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IAuthTokenService _authTokenService;
		private readonly IApplicationDbContext _context;

		public UsersService(UserManager<ApplicationUser> userManager, IAuthTokenService authTokenService, IApplicationDbContext context)
        {
            _userManager = userManager;
            _authTokenService = authTokenService;
            _context = context;
        }

        public async Task<List<ApplicationUser>> GetListAsyncAsync()
		{
			return await _userManager.Users.ToListAsync();
		}

		public async Task<ApplicationUser> GetByIdAsync(string userId)
		{
			return await _userManager.FindByIdAsync(userId);
		}

		public async Task<bool> IsAdminAsync(ApplicationUser user)
		{
			return await _userManager.IsInRoleAsync(user, Role.Admin);
		}

		public async Task<ApplicationUser> GetByAuthTokenAsync(string token, AuthTokenType type)
		{
			var authToken = await _context.AuthTokens.FirstOrDefaultAsync(x => x.Token == token && x.Type == type);

			if (authToken != null)
				return authToken.User;

			return null;
		}

		public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
		{
			return await _userManager.CreateAsync(user, password);
		}

		public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string token, AuthTokenType tokenType)
		{
			var identity = await _userManager.CreateAsync(user);

			if (identity.Succeeded)
			{
				await _authTokenService.AddToken(user.Id, token, tokenType);
			}

			return identity;
		}

		public async Task<TelegramUser> AddTelegramUserInfo(TelegramUser telegramUser)
        {
			_context.TelegramUsers.Add(telegramUser);

			await _context.SaveChangesAsync();

			return telegramUser;
        }

		public async Task<IdentityResult> ChangePassword(string userId, string currentPassword, string newPassword)
		{
			var user = await _userManager.FindByIdAsync(userId);

			var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

			return result;
		}
	}
}
