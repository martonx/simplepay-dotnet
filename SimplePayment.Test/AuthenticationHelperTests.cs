namespace SimplePayment.Test;

public class AuthenticationHelperTests
{
    private readonly JsonSerializerOptions options = new JsonSerializerOptions {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    [Test]
    public async Task MatchExampleStringWithSerializedJsonTest()
    {
        var expectedMessage = await File.ReadAllTextAsync("TestJsonFiles/Message.txt");
        var res = JsonSerializer.Deserialize<OrderDetails>(
            await File.ReadAllTextAsync("TestJsonFiles/AuthOrderDetails.json"));
        var message = JsonSerializer.Serialize(res, options);
        Assert.That(expectedMessage, Is.EqualTo(message));
    }

    [Test]
    public async Task HMACSHA384EncodeCorrectlyWorkingTest()
    {
        var res = JsonSerializer.Deserialize<OrderDetails>(await File.ReadAllTextAsync("TestJsonFiles/AuthOrderDetails.json"));
        var message = JsonSerializer.Serialize(res, options);
        message = message.Replace("/", @"\/");
        var helper = new AuthenticationHelper();
        var merchantKey = "FxDa5w314kLlNseq2sKuVwaqZshZT5d6";
        var signature = helper.HMACSHA384Encode(merchantKey, message);
        var expected = "zXAB58TJdfpDaMa2rXUlefFYVlQQ91CXqot2Y6kcG79wMh55uv1hQphH9xt7qHFn";
        Assert.That(expected, Is.EqualTo(signature));
    }

    [Test]
    public void GenerateSaltCorrectLengthTest()
    {
        var helper = new AuthenticationHelper();
        var salt = helper.GenerateSalt();
        Assert.That(32, Is.EqualTo(salt.Length));
    }

    [Test]
    public void IpnCallbackSignatureHandledCorrectlyWorkingTest()
    {
        var helper = new AuthenticationHelper();
        var secretKey = "D5HrF1fyQ1joLmNO0yRS4m498iZyf32m";
        var signature = "6u2xMr8bDWXfXinNqP0Nu6fndcrEpSUHHM8B7wuOB8U8CehcX65DjHJgZO3XsH6e";
        var message = "{\"salt\":\"0XTzyPrD0P7leRv4TtIef4n3tWhsFDhO\",\"orderRef\":\"traveller-138\",\"method\":\"CARD\",\"merchant\":\"S002203\",\"finishDate\":\"2020-04-07T21:42:14+02:00\",\"paymentDate\":\"2020-04-07T21:42:14+02:00\",\"transactionId\":10253948,\"status\":\"AUTHORIZED\"}";
        var result = helper.IsMessageValid(secretKey, message, signature);
        Assert.That(result, Is.True);
    }
}
