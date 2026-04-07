using System.Net;
using System.Threading.Tasks;

namespace ProductManagement_ApiTest_20260408;

public class DashBoardTest
{
    private string baseUrl = "http://localhost:5292/";
    private HttpClient client;

    [SetUp]
    public void Setup()
    {
        client = new HttpClient();
    }

    [Test]
    public async Task GetDashboard_ValidValue_ReturnOk()
    {
        var response = await client.GetAsync(baseUrl + $"api/dashboard");
        var json = await response.Content.ReadAsStringAsync();
        var data = json.FromJson<Dashboard>();

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(data.RecentOperations.Any(), Is.True);
    }

    [TearDown]
    public void TearDown()
    {
        client.Dispose();
    }
}
