using BLL.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleRestClient
{
    class Program
    {
        private static readonly string _api = "http://localhost:5000/api/";
        private static readonly HttpClient _client = new HttpClient();

        static async Task Main()
        {
            await RunAsync();
        }

        private static async Task RunAsync()
        {
            ShowProducts(await GetAsync<ProductDTO>($"{_api}products"));
            ShowCategories(await GetAsync<CategoryDTO>($"{_api}categories"));

            Console.ReadLine();
        }

        private static async Task<IEnumerable<T>> GetAsync<T>(string path)
        {
            IEnumerable<T> list = null;

            var response = await _client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                list = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync());
            }

            return list;
        }

        private static void ShowProducts(IEnumerable<ProductDTO> products)
        {
            Console.WriteLine("Products");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.ProductName}\t{product.CompanyName}\t{product.CategoryName}");
            }
        }

        private static void ShowCategories(IEnumerable<CategoryDTO> categories)
        {
            Console.WriteLine("Categories");
            foreach (var category in categories)
            {
                Console.WriteLine($"{category.CategoryName}\t{category.Description}");
            }
        }
    }


}
