using ASPApp.Hubs;
using ASPApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ASPApp.Controllers
{
    public class HomeController : Controller
    {
        IHubContext<AnimationHub> hubContext;

        public HomeController(IHubContext<AnimationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }



        //метод для опроса всех подключенных клиентов, создаем еще 1 клиент и делаем отправку/ можно через JS
        [HttpPost]
        public async Task<IActionResult> Push(string message)
        {
            HubConnection tmp = new HubConnectionBuilder()
                 .WithUrl("http://192.168.0.92:1038/animation") //указать свой хаб
                 .Build();

            await tmp.StartAsync();
            await tmp.SendAsync("SendMessage", message);

            Debug.WriteLine("The survey...");
            return RedirectToAction("Index");

        } 

        public FileResult GetBundle()
        {
            //у бандла нет расширения, передавать только в массиве байтов и юзать абсолютный путь 
            byte[] fileBytes = System.IO.File.ReadAllBytes(@"E:\Unity\WebApp\ASPApp\Files\content");
            string fileName = "content";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
