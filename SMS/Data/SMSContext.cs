using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Models;

namespace SMS.Data;

public class SMSContext : IdentityDbContext<SMSUser>
{
    public SMSContext(DbContextOptions<SMSContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<EventCategory> EventCategories { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}