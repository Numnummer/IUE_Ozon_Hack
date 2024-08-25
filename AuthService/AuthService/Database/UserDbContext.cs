using AuthMicroservice.Models.Auth.AuthTokens;
using AuthMicroservice.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthMicroservice.Database
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUser>(usr => usr.HasKey(x => x.Id));
        }
    }
}
