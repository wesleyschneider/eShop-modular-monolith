using System.Text.Json.Serialization;
using MediatR;

namespace eShop.SharedKernel;

public record IntegrationEvent : INotification
{
    public IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    [JsonInclude]
    public Guid Id { get; set; }

    [JsonInclude]
    public DateTime CreationDate { get; set; }
}
