using MediatR;
using Microsoft.EntityFrameworkCore;
using Auditt.Application.Domain;
using Auditt.Application.Domain.Entities;
using Auditt.Application.Infrastructure.Sqlite.Configurations;

namespace Auditt.Application.Infrastructure.Sqlite;

public class AppDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public AppDbContext(DbContextOptions options, IPublisher publisher) : base(options)
    {
        this._publisher = publisher;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Functionary> Functionaries => Set<Functionary>();
    public DbSet<Institution> Institutions => Set<Institution>();
    public DbSet<DataCut> DataCuts => Set<DataCut>();
    public DbSet<Guide> Guides => Set<Guide>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Scale> Scales => Set<Scale>();
    public DbSet<Equivalence> Equivalences => Set<Equivalence>();
    public DbSet<Valuation> Valuations => Set<Valuation>();
    public DbSet<Log> Logs => Set<Log>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<SettingUser> SettingUsers => Set<SettingUser>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Patient> Patients => Set<Patient>();


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            var events = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();

            foreach (var @event in events)
            {
                @event.IsPublished = true;
                //_logger.LogInformation("New domain event {Event}", @event.GetType().Name);

                // Note: If an unhandled exception occurs, all the saved changes will be rolled back
                // by the TransactionBehavior. All the operations related to a domain event finish
                // successfully or none of them do.
                // Reference: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation#what-is-a-domain-event
                await _publisher.Publish(@event);
            }

            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new DataCutConfiguration());
        modelBuilder.ApplyConfiguration(new RolesConfiguration());
        modelBuilder.ApplyConfiguration(new AssessmentConfiguration());
        modelBuilder.ApplyConfiguration(new InstitutionConfiguration());
    }

}

