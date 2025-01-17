﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TSharp.UnitOfWorkGenerator.EFCore.Utils;

namespace TSharp.UnitOfWorkGenerator.DataAccess.Entities;
[UoWDefineDbContext]
public class TSharpContext : DbContext
{
    private readonly IConfiguration _config;

    public TSharpContext(IConfiguration config) : base()
    {
        _config = config;
    }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public DbSet<Employee> Employee { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
            _config.GetConnectionString("TSharpContext"));
        }
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
    }
}
