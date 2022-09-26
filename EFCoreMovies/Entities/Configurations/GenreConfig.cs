﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreMovies.Entities.Configurations;

public class GenreConfig : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        // builder.HasKey(p => p.Id); // for demonstration purposes only, not needed when following EF conventions
        builder.Property(p => p.Name).IsRequired();
    }
}