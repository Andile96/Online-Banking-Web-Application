using Microsoft.AspNetCore.Identity;

namespace UFS_QQ_Bank.Infrastructure
{
    public class CustomUserValidator : IUserValidator<IdentityUser>
    {
        private static readonly string[] _allowedDomains = new[] { "ufs.com", "ufs4life.ac.za", "Gmail.com","yahoo.com" };
        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager,
           IdentityUser user)
        {
            if (_allowedDomains.Any(domain => user.Email.ToLower().EndsWith($"@{domain}")))
            {
                return Task.FromResult(IdentityResult.Success);
            }
            else
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "EmailDomainError",
                    Description = "Email address domain not allowed"
                }));
            }
        }
    }
}
