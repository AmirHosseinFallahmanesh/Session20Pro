using Demo01.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo01.Infrastruture.Identity
{
    public class MyUserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (user.UserName.Contains("@"))
            {
                errors.Add(new IdentityError()
                {
                    Code = "UsernameContainsSequnce",
                    Description = "Username same As Email"
                });
            }

            return Task.FromResult(errors.Count == 0 ? IdentityResult.Success :
                IdentityResult.Failed(errors.ToArray()));
        }
    }
}
