using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server;

public partial class MallenomContext : IdentityDbContext<LoginModel, IdentityRole<int>, int>
{
    public MallenomContext(DbContextOptions<MallenomContext> options)
        : base(options)
    {
    }
}
