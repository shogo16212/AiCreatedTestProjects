using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement_ApiTest_20260408;

public class HistoryTest
{
    private string baseUrl = "http://localhost:5292/";
    private HttpClient client;

    [SetUp]
    public void Setup()
    {
            client  = new HttpClient();
    }

    [Test]
    public async Task GetHistory_ValidValue_ReturnOk()
    {
        var response = await client.GetAsync(baseUrl + "api/history?limit=3");
        var json = await response.Content.ReadAsStringAsync();
        var history = json.FromJson<List<Product>>();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(history.Any(), Is.True);
        Assert.That(history.Count(), Is.EqualTo(3));
    }
    [Test]
    public async Task GetHistory_LimitIsRequire_ReturnBadRequest()
    {
        var response = await client.GetAsync(baseUrl + "api/history");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    [Test]
    public async Task GetHistory_LimitIsOverListCount_ReturnBadRequest()
    {
        var response = await client.GetAsync(baseUrl + "api/history?limit=100");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task PostHistory_ValidValue_ReturnOk()
    {
        var postHistory = await CreateData.CreateHistory(client, baseUrl);

        var content = new StringContent(postHistory.ToJson(), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(baseUrl + "api/history", content);
        var json = await response.Content.ReadAsStringAsync();
        var data = json.FromJson<History>();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task PostHistory_BodyAndProductIdAndActionTypeAreRequire_ReturnBadRequest()
    {
        var postHistory = await CreateData.CreateHistory(client, baseUrl);

        var content = new StringContent(postHistory.ToJson(), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(baseUrl + "api/history", null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));


        postHistory = await CreateData.CreateHistory(client, baseUrl);
        postHistory.ProductId = null;

        content = new StringContent(postHistory.ToJson(), Encoding.UTF8, "application/json");
        response = await client.PostAsync(baseUrl + "api/history", content);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));


        postHistory = await CreateData.CreateHistory(client, baseUrl);
        postHistory.ActionType = null;

        content = new StringContent(postHistory.ToJson(), Encoding.UTF8, "application/json");
        response = await client.PostAsync(baseUrl + "api/history", content);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task PostHistory_AmountAndMemoIsNotRequire_ReturnCreate()
    {
        var postHistory = await CreateData.CreateHistory(client, baseUrl);
        postHistory.Amount = null;

        var content = new StringContent(postHistory.ToJson(), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(baseUrl + "api/history", content);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));


        postHistory = await CreateData.CreateHistory(client, baseUrl);
        postHistory.Memo = null;

        content = new StringContent(postHistory.ToJson(), Encoding.UTF8, "application/json");
        response = await client.PostAsync(baseUrl + "api/history", content);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

    }

    [TearDown]
    public void TearDown()
    {
        client.Dispose();
    }

}
