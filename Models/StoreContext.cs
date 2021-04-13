using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IntelviaStoreAPI.Models
{
    public class StoreContext : IdentityDbContext<UserData>
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
            
        }

        public DbSet<UserData> UserInfo { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //        .Entity<Register>(
        //            eb =>
        //            {
        //                eb.HasNoKey();
        //            });
        //}



        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    builder.HasDefaultSchema("Identity");
        //    builder.Entity<IdentityUser>(entity =>
        //    {
        //        entity.ToTable(name: "User");
        //    });
        //    builder.Entity<IdentityRole>(entity =>
        //    {
        //        entity.ToTable(name: "Role");
        //    });
        //    builder.Entity<IdentityUserRole<string>>(entity =>
        //    {
        //        entity.ToTable("UserRoles");
        //    });
        //    builder.Entity<IdentityUserClaim<string>>(entity =>
        //    {
        //        entity.ToTable("UserClaims");
        //    });
        //    builder.Entity<IdentityUserLogin<string>>(entity =>
        //    {
        //        entity.ToTable("UserLogins");
        //    });
        //    builder.Entity<IdentityRoleClaim<string>>(entity =>
        //    {
        //        entity.ToTable("RoleClaims");
        //    });
        //    builder.Entity<IdentityUserToken<string>>(entity =>
        //    {
        //        entity.ToTable("UserTokens");
        //    });
        //}
    }
}
