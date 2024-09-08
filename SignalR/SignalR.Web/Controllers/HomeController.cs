using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using SignalR.Web.Models;
using System.Diagnostics;

namespace SignalR.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HubConnection _hubConnection;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7233/chathub") // API URL'nizi buraya ekleyin
                .Build();

            _hubConnection.Reconnected += async (connectionId) =>
            {
                // Baðlantý yeniden kurulduðunda yapýlacak iþlemler
                await Task.CompletedTask;
            };

            _hubConnection.Closed += async (exception) =>
            {
                // Baðlantý kapandýðýnda yapýlacak iþlemler
                await Task.Delay(new Random().Next(0, 5) * 1000); // Rastgele gecikme
                await _hubConnection.StartAsync();
            };

            // Baðlantýyý baþlat
            _hubConnection.StartAsync().GetAwaiter().GetResult();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {
            try
            {
                if (_hubConnection.State == HubConnectionState.Disconnected)
                {
                    await _hubConnection.StartAsync();
                }

                await _hubConnection.InvokeAsync("SendMessage", message.User, message.Content);
            }
            catch (Exception ex)
            {
                // Hata ile ilgili loglama veya kullanýcýya bilgi verme
                Console.WriteLine($"Error sending message: {ex.Message}");
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
