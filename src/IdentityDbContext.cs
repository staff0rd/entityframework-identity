// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace Atquin.EntityFramework.Identity
{
    public class IdentityDbContext : IdentityDbContext<IdentityUser, IdentityRole> { }

    public class IdentityDbContext<TUser> : IdentityDbContext<TUser, IdentityRole> where TUser : IdentityUser
    {
        public IdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {

        }

        protected IdentityDbContext()
        {

        }
    }

    public class IdentityDbContext<TUser, TRole> : DbContext
        where TUser : IdentityUser
        where TRole : IdentityRole
    {
        public IdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            
        }

        protected IdentityDbContext()
        {
            
        }

        public DbSet<TUser> Users { get; set; }
        public DbSet<IdentityUserClaim> UserClaims { get; set; }
        public DbSet<IdentityUserLogin> UserLogins { get; set; }
        public DbSet<IdentityUserRole> UserRoles { get; set; }
        public DbSet<TRole> Roles { get; set; }
        public DbSet<IdentityRoleClaim> RoleClaims { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<TUser>().HasKey(u => u.Id);
            builder.Entity<TUser>().Property(u => u.NormalizedUserName).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex")));
            builder.Entity<TUser>().Property(u => u.NormalizedEmail).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("EmailIndex")));
            builder.Entity<TUser>().ToTable("AspNetUsers");
            builder.Entity<TUser>().Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            builder.Entity<TUser>().Property(u => u.UserName).HasMaxLength(256);
            builder.Entity<TUser>().Property(u => u.NormalizedUserName).HasMaxLength(256);
            builder.Entity<TUser>().Property(u => u.Email).HasMaxLength(256);
            builder.Entity<TUser>().Property(u => u.NormalizedEmail).HasMaxLength(256);
            builder.Entity<TUser>().HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            builder.Entity<TUser>().HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            builder.Entity<TUser>().HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);



            builder.Entity<TRole>().HasKey(r => r.Id);
            builder.Entity<TRole>().Property(u => u.NormalizedName).HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex")));
            builder.Entity<TRole>().ToTable("AspNetRoles");
            builder.Entity<TRole>().Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
            builder.Entity<TRole>().Property(u => u.Name).HasMaxLength(256);
            builder.Entity<TRole>().Property(u => u.NormalizedName).HasMaxLength(256);
            builder.Entity<TRole>().HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
            builder.Entity<TRole>().HasMany(r => r.Claims).WithRequired().HasForeignKey(rc => rc.RoleId);

            builder.Entity<IdentityUserClaim>().HasKey(uc => uc.Id);
            builder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");

            builder.Entity<IdentityRoleClaim>().HasKey(rc => rc.Id);
            builder.Entity<IdentityRoleClaim>().ToTable("AspNetRoleClaims");

            builder.Entity<IdentityUserRole>().HasKey(r => new { r.UserId, r.RoleId });
            builder.Entity<IdentityUserRole>().ToTable("AspNetUserRoles");

            // Blocks delete currently without cascade
            //.ForeignKeys(fk => fk.ForeignKey<TUser>(f => f.UserId))
            //.ForeignKeys(fk => fk.ForeignKey<TRole>(f => f.RoleId));

            builder.Entity<IdentityUserLogin>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            builder.Entity<IdentityUserLogin>().ToTable("AspNetUserLogins");
        }
    }
}