namespace SimplePayment.Test;

public class Tests
{
    private readonly JsonSerializerOptions customJsonOptions = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        Converters =
        {
            new CustomJsonConverter.DateTimeConverter(),
            new CustomJsonConverter.IntToStringConverter(),
            new CustomJsonConverter.LongToStringConverter()
        }
    };

    [Test]
    public void TestBillingDetailModel()
    {
        var billingDetailsJson = ReadJson("BillingDetails");
        var billingDetails = JsonSerializer.Deserialize<BillingDetails>(billingDetailsJson, customJsonOptions);

        Assert.That(billingDetails.Name, Is.EqualTo("Teszt Béla"));
        Assert.That(billingDetails.Company, Is.EqualTo("Teszt Kft."));
        Assert.That(billingDetails.Country, Is.EqualTo("Magyarország"));
        Assert.That(billingDetails.City, Is.EqualTo("Szeged"));
        Assert.That(billingDetails.State, Is.EqualTo("Csongrád"));
        Assert.That(billingDetails.Zip, Is.EqualTo("6722"));
        Assert.That(billingDetails.Address, Is.EqualTo("Teszt utca 7."));
        Assert.That(billingDetails.Address2, Is.EqualTo("2 / 3"));
        Assert.That(billingDetails.Phone, Is.EqualTo("36701234567"));
    }

    [Test]
    public void TestIPNModel()
    {
        var IPNJson = ReadJson("IPN");
        var IPN = JsonSerializer.Deserialize<IPNModel>(IPNJson, customJsonOptions); 

        Assert.That(IPN.Salt, Is.EqualTo("223G0O18VAqdLhQYbJz73adT36YzLtak"));
        Assert.That(IPN.OrderRef, Is.EqualTo("101010515363456734591"));
        Assert.That(IPN.Method, Is.EqualTo("CARD"));
        Assert.That(IPN.Merchant, Is.EqualTo("PUBLICTESTHUF"));
        Assert.That(IPN.FinishDate, Is.EqualTo(DateTime.Parse("2018-09-07T20:46:18+0200")));
        Assert.That(IPN.PaymentDate, Is.EqualTo(DateTime.Parse("2018-09-07T20:41:13+0200")));
        Assert.That(IPN.TransactionId, Is.EqualTo(99310118));
        Assert.That(IPN.Status, Is.EqualTo(PaymentStatus.FINISHED));

    }

    [Test]
    public void TestPaymentResponseModel()
    {
        var paymentResponseJson = ReadJson("PaymentResponse");
        var paymentResponse = JsonSerializer.Deserialize<PaymentResponse>(paymentResponseJson, customJsonOptions);

        Assert.That(paymentResponse.ResponseCode, Is.EqualTo(0));
        Assert.That(paymentResponse.TransactionId, Is.EqualTo(99310118));
        Assert.That(paymentResponse.Event, Is.EqualTo("SUCCESS"));
        Assert.That(paymentResponse.Merchant, Is.EqualTo("PUBLICTESTHUF"));
        Assert.That(paymentResponse.OrderId, Is.EqualTo("101010515363456734591"));
    }

    [Test]
    public void TestOrderDetailsModel()
    {
        var orderDetailsJson = ReadJson("OrderDetails");
        var orderDetails = JsonSerializer.Deserialize<OrderDetails>(orderDetailsJson, customJsonOptions);

        Assert.That(orderDetails.Merchant, Is.EqualTo("PUBLICTESTHUF"));
        Assert.That(orderDetails.OrderRef, Is.EqualTo("101010515363456734591"));
        Assert.That(orderDetails.CustomerEmail, Is.EqualTo("sdk_test@otpmobil.com"));
        Assert.That(orderDetails.Language, Is.EqualTo("HU"));
        Assert.That(orderDetails.Currency, Is.EqualTo("HUF"));
        Assert.That(orderDetails.Total, Is.EqualTo("100"));
        Assert.That(orderDetails.TwoStep, Is.EqualTo(true));
        Assert.That(orderDetails.Salt, Is.EqualTo("d471d2fb24c5a395563ff60f8ba769d1"));
        Assert.That(orderDetails.Methods[0], Is.EqualTo("CARD"));
        Assert.That(orderDetails.Invoice.ToString(), Is.EqualTo(JsonSerializer.Deserialize<BillingDetails>(ReadJson("BillingDetails")).ToString()));
        Assert.That(orderDetails.Timeout, Is.EqualTo("2018-09-07T20:51:13+00:00"));
        Assert.That(orderDetails.Url, Is.EqualTo("http://simplepaytestshop.hu/back.php"));
        Assert.That(orderDetails.SDKVersion, Is.EqualTo("SimplePay_PHP_SDK_2.0_180906"));
        Assert.That(orderDetails.OrderItems[0].ToString(), Is.EqualTo(JsonSerializer.Deserialize<OrderItem>(ReadJson("OrderItem")).ToString()));
    }

    [Test]
    public void TestFinalResponseModelCorrectDecimalDeserialize()
    {
        var json = ReadJson("FinishRequestInput");
        var result = JsonSerializer.Deserialize<FinishResponse>(json, CustomJsonConverter.CustomJsonOptions());
        Assert.That(result.ApproveTotal, Is.EqualTo(1.01));
    }

    [Test]
    public void TestOrderItemModel()
    {
        var orderItemJson = ReadJson("OrderItem");
        var orderItem = JsonSerializer.Deserialize<OrderItem>(orderItemJson, customJsonOptions);

        Assert.That(orderItem.Ref, Is.EqualTo("Nagy Usa Körút"));
        Assert.That(orderItem.Title, Is.EqualTo("szép út"));
        Assert.That(orderItem.Description, Is.EqualTo("nagyon szép"));
        Assert.That(orderItem.Amount, Is.EqualTo(1));
        Assert.That(orderItem.Price, Is.EqualTo(959000));
        Assert.That(orderItem.Tax, Is.EqualTo(210000));
    }

    [Test]
    public void TestTransactionResponse()
    {
        var json = ReadJson("TransactionResponse");
        var response = JsonSerializer.Deserialize<TransactionResponse>(json, customJsonOptions);
        Assert.That(response.PaymentUrl, Is.Not.Null);
    }

    private string RemoveWhiteSpace(string json) => 
        Regex.Replace(json, @"(""[^""\\]*(?:\\.[^""\\]*)*"")|\s+", "$1");

    private string ReadJson(string jsonFile)
    {
        var jsonResult = File.ReadAllText($@"TestJsonFiles/{jsonFile}Json.json");

        return RemoveWhiteSpace(jsonResult);
    }
}