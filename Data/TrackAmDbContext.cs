using Microsoft.EntityFrameworkCore;

public class TrackAmDbContext : DbContext
{
    public TrackAmDbContext(DbContextOptions<TrackAmDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Expense>()
    .Property(e => e.Amount)
    .HasColumnType("decimal(18,2)");


        // Seed Users

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@example.com",
                GoogleId = "google-12345",
                CreatedAt = new DateTime(2024, 1, 1)    

            },
            new User
            {
                Id = 2,
                Username = "janedoe",
                Email = "jane@example.com",
                GoogleId = "google-67890",
                CreatedAt = new DateTime(2024, 1, 2)
            }
        );

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Food & Dining",
                Description = "Restaurants, groceries and food expenses",
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new Category
            {
                Id = 2,
                Name = "Transport",
                Description = "Fuel, public transport and travel expenses",
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new Category
            {
                Id = 3,
                Name = "Entertainment",
                Description = "Movies, games and leisure activities",
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new Category
            {
                Id = 4,
                Name = "Shopping",
                Description = "Clothing, electronics and general shopping",
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new Category
            {
                Id = 5,
                Name = "Health",
                Description = "Medical bills, pharmacy and fitness expenses",
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new Category
            {
                Id = 6,
                Name = "Bills & Utilities",
                Description = "Electricity, water, internet and phone bills",
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new Category
            {
                Id = 7,
                Name = "Education",
                Description = "Books, courses and tuition fees",
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 1)
            },
            new Category
            {
                Id = 8,
                Name = "Others",
                Description = "Miscellaneous expenses",
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 1)
            }
        );

    
        modelBuilder.Entity<Expense>()
        .HasData(
            new Expense
            {
                Id = 1,
                Description = "Grocery shopping",
                Amount = 150.00m,
                CategoryId = 1,
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 5),
                UpdatedAt = new DateTime(2024, 1, 5)
            },
            new Expense
            {
                Id = 2,
                Description = "Bus fare",
                Amount = 20.00m,
                CategoryId = 2,
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 6),
                UpdatedAt = new DateTime(2024, 1, 6)
            },
            new Expense
            {
                Id = 3,
                Description = "Netflix subscription",
                Amount = 45.00m,
                CategoryId = 3,
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 7),
                UpdatedAt = new DateTime(2024, 1, 7)
            },
            new Expense
            {
                Id = 4,
                Description = "Electricity bill",
                Amount = 200.00m,
                CategoryId = 6,
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 8),
                UpdatedAt = new DateTime(2024, 1, 8)
            },
            new Expense
            {
                Id = 5,
                Description = "Doctor visit",
                Amount = 80.00m,
                CategoryId = 5,
                UserId = 1,
                CreatedAt = new DateTime(2024, 1, 9),
                UpdatedAt = new DateTime(2024, 1, 9)
            }
        );
    }
}