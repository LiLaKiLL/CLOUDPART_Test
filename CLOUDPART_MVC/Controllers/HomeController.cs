using CLOUDPART_MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CLOUDPART_MVC.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient httpClient = new HttpClient();
        static string ApiUrl = "http://localhost:5179/api/Product";
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost("/get/{productName?}")]
        public async Task<JsonResult> Get(string? productName)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();
            var response = await httpClient.GetAsync(ApiUrl + $"/{productName}");
            if (response.IsSuccessStatusCode)
            {
                var jsoncontent = await response.Content.ReadAsStringAsync();
                products = System.Text.Json.JsonSerializer.Deserialize<List<ProductViewModel>>(jsoncontent);
            }
            return Json(products);
        }
        [HttpPost("/create")]
        public async Task<JsonResult> Create([FromBody] ProductViewModel product)
        {
            var response = await httpClient.PostAsync(ApiUrl, JsonContent.Create(product));
            return Json(new { result_status = response.StatusCode, result_message = response.Content.ReadAsStringAsync() });
        }
        [HttpPost("/update")]
        public async Task<JsonResult> Update([FromBody] ProductViewModel product)
        {
            var response = await httpClient.PutAsync(ApiUrl, JsonContent.Create(product));
            return Json(new { result_status = response.StatusCode, result_message = response.Content.ReadAsStringAsync() });
        }
        [HttpGet("/delete/{id?}")]
        public async Task<JsonResult> Delete(Guid? id)
        {
            var response = await httpClient.DeleteAsync(ApiUrl + $"/{id}");
            return Json(new { result_status = response.StatusCode, result_message = response.Content.ReadAsStringAsync() });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
