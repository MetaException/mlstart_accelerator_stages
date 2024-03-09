using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace server;

public partial class MallenomContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
{
    public MallenomContext(DbContextOptions<MallenomContext> options)
        : base(options)
    {
    }
}