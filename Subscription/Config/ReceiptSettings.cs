namespace Skad.Subscription.Config;

public class ReceiptSettings
{
    public bool UseTmpDir { get; set; } = false;
    public string? ReceiptDir { get; set; }
}