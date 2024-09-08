using Microsoft.AspNetCore.SignalR;
using SignalR.API.Data.Entity;
using SignalR.API.Data;

namespace SignalR.API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            // Veritabanına mesajı ekle
            var newMessage = new Message
            {
                User = user,
                Content = message,
                Timestamp = DateTime.Now
            };

            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            // Mesajı tüm bağlı istemcilere gönder
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}