using System.Collections.Generic;

namespace Skad.Subscription.Domain.Service;

public class InMemoryReceiptManager : IReceiptManager
{
    public string? Receipt { get; private set; }

    public bool HasReceipt => Receipt != null;

    public void AddReceipt(Data.Model.Subscription subscription)
    {
        Receipt = FormatHtmlReceipt(subscription);
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