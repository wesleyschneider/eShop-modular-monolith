namespace eShop.Webhooks.Module.Model;

public class WebhookSubscription
{
    public int Id { get; set; }

    public WebhookType Type { get; set; }
    public DateTime Date { get; set; }
    [Required]
    public string DestUrl { get; set; } = default!;
    public string? Token { get; set; }
    [Required]
    public string UserId { get; set; } = default!;
}
