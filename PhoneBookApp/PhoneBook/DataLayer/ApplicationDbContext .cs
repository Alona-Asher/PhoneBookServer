public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ContactEntity> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<ContactEntity>().ToTable("Contacts");
        modelBuilder.Entity<ContactEntity>().HasKey(c => c.Id);
        modelBuilder.Entity<ContactEntity>().Property(c => c.FirstName).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<ContactEntity>().Property(c => c.LastName).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<ContactEntity>().Property(c => c.Phone).IsRequired().HasMaxLength(15);
        modelBuilder.Entity<ContactEntity>().Property(c => c.Address).HasMaxLength(100);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}

