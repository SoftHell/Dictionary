using System;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        
    }
}