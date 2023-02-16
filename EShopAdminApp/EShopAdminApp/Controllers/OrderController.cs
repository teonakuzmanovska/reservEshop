using ClosedXML.Excel;
using EShopAdminApp.Models;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EShopAdminApp.Controllers
{
    public class OrderController : Controller
    {
        public OrderController()
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44359/api/AdminApi/GetAllActiveOrders";

            HttpResponseMessage responseMessage = client.GetAsync(URL).Result;

            var data = responseMessage.Content.ReadAsAsync<List<Order>>().Result;

            return View(data);
        }
        public IActionResult GetOrderDetails(int id)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44359/api/AdminApi/GetOrderDetails";

            var model = new
            {
                Id = id
            };

            // string because JSON in backend is essentially a string
            // content serializes our model as a json so it can send it to a different api
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(URL, httpContent).Result;

            var data = responseMessage.Content.ReadAsAsync<Order>().Result;

            return View(data);
        }

        public FileResult SavePdf(int id)
        {
            HttpClient client = new HttpClient();
            string URL = "https://localhost:44359/api/AdminApi/GetOrderDetails";

            var model = new
            {
                Id = id
            };

            // string because JSON in backend is essentially a string
            // content serializes our model as a json so it can send it to a different api
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(URL, httpContent).Result;

            var result = responseMessage.Content.ReadAsAsync<Order>().Result;
            var directoryPath = Directory.GetCurrentDirectory(); // for debbuging purposes
            // always do this, not hardcoding
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");

            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{Username}}", result.OrderedBy.Email);
            

            StringBuilder stringBuilder = new StringBuilder();

            var totalPrice = 0;

            foreach( var item in result.Products)
            {
                totalPrice += item.Quantity * item.Product.ProductPrice;
                stringBuilder.AppendLine(item.Product.ProductName + " with quantity of: " + item.Quantity + " and price of: " + item.Product.ProductPrice);

            }
            document.Content.Replace("{{ProductList}}", stringBuilder.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString());

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());

            //DocxSaveOptions() i .docx za word dokument

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }

        [HttpGet]
        public FileContentResult ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");
                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Customer email";

                HttpClient client = new HttpClient();
                string URL = "https://localhost:44359/api/AdminApi/GetAllActiveOrders";

                HttpResponseMessage responseMessage = client.GetAsync(URL).Result;

                var data = responseMessage.Content.ReadAsAsync<List<Order>>().Result;

                for(int i = 1; i < data.Count(); i++)
                {
                    var item = data[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.OrderedBy.Email;

                    for(int p=1; p <= item.Products.Count(); p++)
                    {
                        worksheet.Cell(1, p + 3).Value = "Product-" + (p);
                        worksheet.Cell(i + 1, p + 3).Value = item.Products.ElementAt(p - 1).Product.ProductName;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }
        }
    }
}
