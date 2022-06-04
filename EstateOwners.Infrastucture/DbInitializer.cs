using EstateOwners.Domain;
using EstateOwners.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EstateOwners.Infrastucture
{
    public class DbInitializer
	{
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _services;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(IServiceProvider services)
        {  
			_services = services;

            _context = _services.GetRequiredService<ApplicationDbContext>();
			_userManager = _services.GetRequiredService<UserManager<ApplicationUser>>();
			_roleManager = _services.GetRequiredService<RoleManager<IdentityRole>>();
			_logger = _services.GetRequiredService<ILogger<DbInitializer>>();
		}

		public async Task Initialize()
		{
			_context.Database.EnsureCreated();

			await CreateDefaultUserAndRole();
		}

		private async Task CreateDefaultUserAndRole()
		{
			const string email = "admin@mail.ru";
			const string password = "admin";

			// Берем первого созданного пользователя
			var user = await _context.Users.FirstOrDefaultAsync();

			// Для пустой базы создаем нового пользователя
			if (user == null)
			{
				user = await CreateDefaultUser(email);

				// Временно отключаем проверку пароля для администратора
				var passwordValidators = _userManager.PasswordValidators;
				_userManager.PasswordValidators.Clear();

				await SetPasswordForUser(email, user, password);

				// Возвращаем проверку пароля обратно
				foreach (var validator in passwordValidators)
					_userManager.PasswordValidators.Add(validator);
			}

			var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == Role.Admin);

			// Создаем группу администратора, если в системе еще не было групп и устанавливаем её для пользователя
			// Если ранее был пользователь но не было группы, то для первого созданного пользователя будет установалена группа администратора
			if (role == null)
			{
				await CreateDefaultAdministratorRole(Role.Admin);

				await AddRoleToUser(email, Role.Admin, user);
			}
		}

		private async Task CreateDefaultAdministratorRole(string administratorRole)
		{
			_logger.LogInformation($"Create the role `{administratorRole}` for application");
			var result = await _roleManager.CreateAsync(new IdentityRole(administratorRole));
			if (result.Succeeded)
			{
				_logger.LogDebug($"Created the role `{administratorRole}` successfully");
			}
			else
			{
				var exception = new ApplicationException($"Default role `{administratorRole}` cannot be created");
				_logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(result));
				throw exception;
			}
		}

		private async Task<ApplicationUser> CreateDefaultUser(string email)
		{
			_logger.LogInformation($"Create default user with email `{email}` for application");
			var user = new ApplicationUser(new Trustee(), email);

			var result = await _userManager.CreateAsync(user);
			if (result.Succeeded)
			{
				_logger.LogDebug($"Created default user `{email}` successfully");
			}
			else
			{
				var exception = new ApplicationException($"Default user `{email}` cannot be created");
				_logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(result));
				throw exception;
			}

			var createdUser = await _userManager.FindByEmailAsync(email);
			return createdUser;
		}

		private async Task SetPasswordForUser(string email, ApplicationUser user, string password)
		{
			_logger.LogInformation($"Set password for default user `{email}`");
			var result = await _userManager.AddPasswordAsync(user, password);
			if (result.Succeeded)
			{
				_logger.LogTrace($"Set password `{password}` for default user `{email}` successfully");
			}
			else
			{
				var exception = new ApplicationException($"Password for the user `{email}` cannot be set");
				_logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(result));
				throw exception;
			}
		}

		private async Task AddRoleToUser(string email, string administratorRole, ApplicationUser user)
		{
			_logger.LogInformation($"Add default user `{email}` to role '{administratorRole}'");
			var result = await _userManager.AddToRoleAsync(user, administratorRole);
			if (result.Succeeded)
			{
				_logger.LogDebug($"Added the role '{administratorRole}' to default user `{email}` successfully");
			}
			else
			{
				var exception = new ApplicationException($"The role `{administratorRole}` cannot be set for the user `{email}`");
				_logger.LogError(exception, GetIdentiryErrorsInCommaSeperatedList(result));
				throw exception;
			}
		}

		private static string GetIdentiryErrorsInCommaSeperatedList(IdentityResult result)
		{
			string errors = null;
			foreach (var identityError in result.Errors)
			{
				errors += identityError.Description;
				errors += ", ";
			}
			return errors;
		}
	}
}
