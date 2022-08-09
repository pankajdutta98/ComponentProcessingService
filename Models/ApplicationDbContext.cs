using Microsoft.EntityFrameworkCore;

namespace ComponentProcessingService.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }        

        public DbSet<ProcessedOrderData> processedOrderData { get; set; }

        public DbSet<PaymentDetails> paymentDetails { get; set; }


    }
}
