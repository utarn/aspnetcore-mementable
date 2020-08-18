using System;
using System.Collections.Generic;
using System.Text;
using aspnetcore_mementable.MementoExtension.Interfaces;
using MementoExtension.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_mementable.Data
{
    public class ApplicationDbContext : IdentityDbContext, StateDbContext
    {
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<State> States { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


    }
}
