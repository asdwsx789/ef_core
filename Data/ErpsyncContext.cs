#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EF_WebApi.Models;

namespace EF_WebApi.Data;

public partial class ErpsyncContext : DbContext
{
    public ErpsyncContext(DbContextOptions<ErpsyncContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UofPerson> uof_people { get; set; }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
