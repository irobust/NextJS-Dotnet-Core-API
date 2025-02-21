using InvoiceBackend.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext {
    public DbSet<Invoice> Invoices {get; set;}
    public DbSet<Product> Products {get; set;}

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<Product>().HasData(
            new Product(){ Id = 6, Name = "Product A", Price=100, InvoiceId = 5},
            new Product(){ Id = 7, Name = "Product B", Price=101, InvoiceId = 5},
            new Product(){ Id = 8, Name = "Product C", Price=102, InvoiceId = 5}
        );
        
        modelBuilder.Entity<Product>()
               .HasOne(p => p.Invoice)
               .WithMany(invoice => invoice.Products)
               .HasForeignKey(invoice => invoice.InvoiceId);
        base.OnModelCreating(modelBuilder);
    }
}