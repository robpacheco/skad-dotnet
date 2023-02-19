using System;
using System.IO;
using Microsoft.Extensions.Options;
using Skad.Subscription.Config;

namespace Skad.Subscription.Domain.Service;

public class FsReceiptManager : IReceiptManager
{
    private readonly ReceiptSettings _receiptSettings;

    public FsReceiptManager(IOptions<ReceiptSettings> receiptSettings)
    {
        _receiptSettings = receiptSettings.Value ?? throw new ArgumentNullException(nameof(receiptSettings));
    }

    public string? Receipt => File.ReadAllText(GetReceiptFilePath());

    public bool HasReceipt => File.Exists(GetReceiptFilePath());

    public void AddReceipt(Data.Model.Subscription subscription)
    {
        var location = GetReceiptFilePath();
        var receipt = FormatHtmlReceipt(subscription);
        File.WriteAllText(location, receipt);
    }

    private string GetReceiptFilePath()
    {
        string location;

        if (_receiptSettings.UseTmpDir || _receiptSettings.ReceiptDir == null)
        {
            location = Path.GetTempPath();
        }
        else
        {
            location = _receiptSettings.ReceiptDir;
        }

        return Path.Combine(location, "receipt.html");
    }
    
    private string FormatHtmlReceipt(Data.Model.Subscription subscription)
    {
        return $@"
        <html>
            <head>
                <title>Receipt for {subscription.CardName}</title>
            </head>
            <body>
                <p>Name: {subscription.CardName}</p>
                <p>Subscription Tier: {subscription.Tier}</p>
                <p>Date Purchased: {subscription.DatePurchased}</p>
                <p>Amount Paid: ${subscription.AmountPaid}</p>
                <p>Expires: {subscription.DateExpires.ToShortDateString()}</p>
            </body>
        </html>
        ";
    }
}