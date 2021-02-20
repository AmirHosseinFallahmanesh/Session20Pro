using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Demo01.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class AppUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }


    }

    public class UserViewModel
    {

        public string Id { get; set; }
        public string Username { get; set; }
        public string  Email { get; set; }
        public string Password { get; set; }


   
    }

}