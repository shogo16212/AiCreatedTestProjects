using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement_ApiTest_20260408
{
    public class ProductTest
    {
        private string baseUrl = "http://localhost:5292/";
        private HttpClient client;
        [SetUp]
        public void Setup()
        {
            client  = new HttpClient();
        }

        [Test]
        public async Task GetProducts_ValidValue_ResponseOk()
        {
            var response = await client.GetAsync(baseUrl + "api/products");
            var json = await response.Content.ReadAsStringAsync();
            var products = json.FromJson<List<Product>>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(products.Any(), Is.True);
        }

        [Test]
        public async Task GetProduct_ValidValue_ResponseOk()
        {
            var getProduct = await GetItem.GetProduct(client, baseUrl);

            var response = await client.GetAsync(baseUrl + $"api/products/{getProduct.ProductId}");
            var json = await response.Content.ReadAsStringAsync();
            var data = json.FromJson<Product>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(data.ProductId, Is.EqualTo(getProduct.ProductId));
        }

        [Test]
        public async Task GetProduct_ProductIdNotExist_ResponseNotFound()
        {
            var response = await client.GetAsync(baseUrl + $"api/products/0");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task PostProduct_ValidValue_ResponseCreated()
        {
            var postProduct = CreateData.CreateProduct();

            var content = new StringContent(postProduct.ToJson(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(baseUrl + $"api/products", content);
            var json = await response.Content.ReadAsStringAsync();
            var data = json.FromJson<Product>();

            var createResponse = await client.GetAsync(baseUrl + $"api/products/{data.ProductId}");
            var createJson = await createResponse.Content.ReadAsStringAsync();
            var createData = createJson.FromJson<Product>();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(data.ProductName, Is.EqualTo(postProduct.ProductName));
            Assert.That(data.Stock, Is.EqualTo(postProduct.Stock));
            Assert.That(data.Price, Is.EqualTo(postProduct.Price));
        }

        [Test]
        public async Task PostProduct_BodyAndProductNameAndStockAndPriceAreRequire_ResponseBadrequest()
        {
            var response = await client.PostAsync(baseUrl + $"api/products", null);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));


            var postProduct = CreateData.CreateProduct();
            postProduct.ProductName = null;

            var content = new StringContent(postProduct.ToJson(), Encoding.UTF8, "application/json");
            response = await client.PostAsync(baseUrl + $"api/products", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));


            postProduct = CreateData.CreateProduct();
            postProduct.Stock = null; 

            content = new StringContent(postProduct.ToJson(), Encoding.UTF8, "application/json");
            response = await client.PostAsync(baseUrl + $"api/products", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));


            postProduct = CreateData.CreateProduct();
            postProduct.Price = null; 

            content = new StringContent(postProduct.ToJson(), Encoding.UTF8, "application/json");
            response = await client.PostAsync(baseUrl + $"api/products", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        }

        [Test]
        public async Task PostProduct_NameIsEmpty_ReturnBadRequest()
        {
            var postProduct = CreateData.CreateProduct();
            postProduct.ProductName = "";

            var content = new StringContent(postProduct.ToJson(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(baseUrl + $"api/products", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        }

        [Test]
        public async Task PutProduct_ValidValue_ReturnOk()
        {
            var putProduct = CreateData.CreateProduct();
            var getProduct = await GetItem.GetProduct(client, baseUrl);

            var content = new StringContent(putProduct.ToJson(), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(baseUrl + $"api/products/{getProduct.ProductId}", content);
            var json = await response.Content.ReadAsStringAsync();
            var data = json.FromJson<Product>();

            var updateResponse = await client.GetAsync(baseUrl + $"api/products/{data.ProductId}");
            var updateJson = await updateResponse.Content.ReadAsStringAsync();
            var updateData = updateJson.FromJson<Product>();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(data.ProductName, Is.EqualTo(updateData.ProductName));
            Assert.That(data.Stock, Is.EqualTo(updateData.Stock));
            Assert.That(data.Price, Is.EqualTo(updateData.Price));
        }

        [Test]
        public async Task PutProduct_BodyAndProductNameAndStockAndPriceAreRequire_ResponseBadrequest()
        {
            var putProduct = CreateData.CreateProduct();
            var getProduct = await GetItem.GetProduct(client, baseUrl);

            var response = await client.PutAsync(baseUrl + $"api/products/{getProduct.ProductId}", null);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));


            putProduct = CreateData.CreateProduct();
            putProduct.ProductName = null;

            var content = new StringContent(putProduct.ToJson(), Encoding.UTF8, "application/json");
            response = await client.PutAsync(baseUrl + $"api/products/{getProduct.ProductId}", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));


            putProduct = CreateData.CreateProduct();
            putProduct.Stock = null;

            content = new StringContent(putProduct.ToJson(), Encoding.UTF8, "application/json");
            response = await client.PostAsync(baseUrl + $"api/products", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));


            putProduct = CreateData.CreateProduct();
            putProduct.Price = null;

            content = new StringContent(putProduct.ToJson(), Encoding.UTF8, "application/json");
            response = await client.PostAsync(baseUrl + $"api/products", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

        }

        [Test]
        public async Task PutProduct_ProductIdNotExist_ResponseNotFound()
        {
            var putProduct = CreateData.CreateProduct();

            var content = new StringContent(putProduct.ToJson(), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(baseUrl + $"api/products/0", content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

        }

        [Test]
        public async Task DeleteProduct_ValidValue_ResponseNoContent()
        {
            var deleteProduct = await GetItem.GetProduct(client, baseUrl);

            var response = await client.DeleteAsync(baseUrl + $"api/products/{deleteProduct.ProductId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        }

        [TearDown]
        public void TearDown()
        {
            client.Dispose();
        }
    }
}
