using System.Collections.Generic;
using DevReviews.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevReviews.API.Persistence
{
    public class DevReviewsDbContext : DbContext
    {
        public DevReviewsDbContext(DbContextOptions<DevReviewsDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Product>(p => {
                p.ToTable("tb_Product");
                p.HasKey(p => p.Id);

                p
                    .HasMany(pp => pp.Reviews)
                    //Caso esteja utilizando a propriedade de navegação
                    // .WithOne(r => r.Product)
                    .WithOne()
                    .HasForeignKey(r => r.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
                

            modelBuilder.Entity<ProductReview>(pr => {
                pr.ToTable("tb_ProductReview");
                pr.HasKey(p => p.Id);

                pr.Property(p => p.Author)
                    .HasMaxLength(50)
                    .IsRequired();
                    // .HasColumnName("txt_Author");
            });
        }
        
        
        
        
    }
}