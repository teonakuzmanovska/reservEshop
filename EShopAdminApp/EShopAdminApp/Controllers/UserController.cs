using EShopAdminApp.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace EShopAdminApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult ImportUsers(IFormFile file)
        {
            //make a copy
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            //read data from uploaded file

            List<User> users = GetUsersFromExcelFile(file.FileName);

            HttpClient client = new HttpClient();

            string URL = "https://localhost:44359/api/AdminApi/ImportAllUsers";

            HttpContent content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json");
           
            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("Index", "Order");
        }

        private List<User> GetUsersFromExcelFile(string fileName)
        {
            string pathToFile = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            
            List<User> userList = new List<User>();

            using (var stream = System.IO.File.Open(pathToFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        userList.Add(new Models.User
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            ConfirmPassword = reader.GetValue(2).ToString()
                        });
                    }
                }
            }

            return userList;

        }
    }
}
