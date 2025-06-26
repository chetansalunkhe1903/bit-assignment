﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.ItemName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasIndex(i => i.ItemName); 

            builder.Property(i => i.Quantity)
                   .IsRequired();

            builder.Property(i => i.ProductId)
                   .IsRequired();

            builder.HasIndex(i => i.ProductId); //

            builder.HasOne(i => i.Product)
                   .WithMany(p => p.Items)
                   .HasForeignKey(i => i.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
