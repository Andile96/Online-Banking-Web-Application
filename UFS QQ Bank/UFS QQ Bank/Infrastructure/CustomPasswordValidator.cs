﻿using Microsoft.AspNetCore.Identity;

namespace UFS_QQ_Bank.Infrastructure
{
    public class CustomPasswordValidator : IPasswordValidator<IdentityUser>
    {
        private static readonly string[] _commonPasswords = new[] {
            "123456", "123456789", "picture1","password","12345678",
            "111111","123123","12345","1234567890","senha","1234567",
            "qwerty","abc123","Million2","000000","1234","iloveyou",
            "aaron431","password1","qqww1122","admin","000000","Admin123","password123\r\n"};
        public Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager,
            IdentityUser user, string password)
        {
            IEnumerable<IdentityError> errors = CheckTop20(password);

            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors = errors.Concat(new[] {new IdentityError
                {
                    Code = "PasswordContainsUserName",
                    Description = "Password cannot contain username"
                } });
            }

            if (password.Contains("12345"))
            {
                errors = errors.Concat(new[] {new IdentityError
                {
                    Code = "PasswordContainsSequence",
                    Description = "Password cannot contain numeric sequence"
                } });
            }

            return Task.FromResult(!errors.Any() ?
              IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }

        private static IEnumerable<IdentityError> CheckTop20(string password)
        {
            if (_commonPasswords.Any(commonPassword =>
                    string.Equals(commonPassword, password,
                        StringComparison.CurrentCultureIgnoreCase)))
            {
                return new[] {new IdentityError {
                        Code = "CommonPassword",
                        Description = "The top 20 passwords cannot be used"
                }};
            }
            return Enumerable.Empty<IdentityError>();
        }
    }
}
