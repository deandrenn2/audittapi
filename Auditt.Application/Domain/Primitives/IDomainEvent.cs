using MediatR;

namespace Auditt.Application.Domain.Primitives;

public interface IDomainEvent : INotification
{
    public int Id { get; init;}
}
