using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();

            try
            {
                // Get All Regions from Web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7271/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex)
            {
                // Log the exception
            }

            return View(response);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7271/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var respose = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (respose is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7271/api/regions/{id.ToString()}");

            if (response is not null)
            {
                return View(response);
            }

            return View(null);
        }


        //[HttpPost]
        //public async Task<IActionResult> Edit(RegionDto request)
        //{
        //    var client = httpClientFactory.CreateClient();

        //    var httpRequestMessage = new HttpRequestMessage()
        //    {
        //        Method = HttpMethod.Put,
        //        RequestUri = new Uri($"https://localhost:7271/api/regions/{request.Id}"),
        //        Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
        //    };

        //    var httpResponseMessage = await client.SendAsync(httpRequestMessage);
        //    httpResponseMessage.EnsureSuccessStatusCode();

        //    var respose = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

        //    if (respose is not null)
        //    {
        //        return RedirectToAction("Edit", "Regions");
        //    }

        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto request)
        {
            if (!ModelState.IsValid)
            {
                return View(request); // show user form errors if any
            }

            var client = httpClientFactory.CreateClient();

            var jsonPayload = JsonSerializer.Serialize(request);
            Console.WriteLine("Payload: " + jsonPayload);

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7271/api/regions/{request.Id}"),
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var error = await httpResponseMessage.Content.ReadAsStringAsync();
                Console.WriteLine("API Error: " + error);
                ModelState.AddModelError(string.Empty, "API Error: " + error);
                return View(request);
            }

            return RedirectToAction("Index", "Regions", new { id = request.Id });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7271/api/regions/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {
                // Console
            }

            return View("Edit");
        }
    }
}