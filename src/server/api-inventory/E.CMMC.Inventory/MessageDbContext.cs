using Microsoft.EntityFrameworkCore;

public class MessageDbContext : DbContext
{
    public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options)
    {
    }

    public DbSet<MessageEntity> Messages { get; set; }
}
