
namespace Auditt.Application.Domain.Primitives;

public abstract record DomainEvent(int Id) : IDomainEvent;
