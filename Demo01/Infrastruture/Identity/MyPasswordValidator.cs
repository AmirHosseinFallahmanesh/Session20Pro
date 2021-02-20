using Demo01.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo01.Infrastruture.Identity
{
    public class MyPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (password.StartsWith("123"))
            {
                errors.Add(new IdentityError()
                {
                    Code = "PasswordContainsSequnce",
                    Description="Password Can not Contain 123"
                });
            }

            return Task.FromResult(errors.Count==0?IdentityResult.Success:
                IdentityResult.Failed(errors.ToArray()));

        }
    }
}
