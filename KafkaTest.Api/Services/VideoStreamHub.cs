using Microsoft.AspNetCore.SignalR;

namespace KafkaTest.Api.Services
{
    public class VideoStreamHub : Hub
    {
        // Método para enviar datos de video a los clientes
        public async Task SendVideoChunk(byte[] videoChunk)
        {
            await Clients.All.SendAsync("ReceiveVideoChunk", videoChunk);
        }
    }
}
