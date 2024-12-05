using Microsoft.AspNetCore.SignalR;
using OpenCvSharp;

namespace KafkaTest.Api.Services
{
    public class VideoStreamService
    {
        private readonly IHubContext<VideoStreamHub> _hubContext;

        public VideoStreamService(IHubContext<VideoStreamHub> hubContext)
        {
            _hubContext = hubContext;
        }

        //public async Task StreamVideoMediaSource()
        //{
        //    var filePath = "C:\\Users\\alvaroa\\Desktop\\videos\\Pelea_de_Monos.mp4";

        //    // Información del video
        //    var fileInfo = new FileInfo(filePath);
        //    var fileSize = fileInfo.Length;
        //    const int bufferSize = 64 * 1024; // 64 KB
        //    var buffer = new byte[bufferSize];

        //    // Enviar metainformación del video (opcional)
        //    await _hubContext.Clients.All.SendAsync("VideoInfo", new
        //    {
        //        FileName = fileInfo.Name,
        //        FileSize = fileSize,
        //        MimeType = "video/mp4"
        //    });

        //    using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        //    int bytesRead;
        //    while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        //    {
        //        var chunk = buffer.Take(bytesRead).ToArray();

        //        // Enviar el chunk al cliente
        //        await _hubContext.Clients.All.SendAsync("ReceiveVideoChunkMediaSource", chunk);

        //        // Control de flujo (simulación)
        //        await Task.Delay(50); // Ajusta según el caso
        //    }

        //    // Notificar al cliente que el streaming ha finalizado
        //    await _hubContext.Clients.All.SendAsync("StreamEnded");
        //}

        public async Task StreamVideoMediaSource()
        {
            var filePath = "C:\\Users\\alvaroa\\Desktop\\videos\\videocorto.mp4";

            // Información del video
            var fileInfo = new FileInfo(filePath);
            const int bufferSize = 64 * 1024; // 64 KB
            var buffer = new byte[bufferSize];

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            int bytesRead;
            while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                var chunk = buffer.Take(bytesRead).ToArray();

                // Enviar el chunk junto con el MIME Type para que el cliente lo sepa
                await _hubContext.Clients.All.SendAsync("ReceiveVideoChunkMediaSource", new
                {
                    MimeType = "video/mp4", // Se asegura de enviar el MIME Type junto con cada chunk
                    Chunk = Convert.ToBase64String(chunk) // Convertir el chunk a base64 para enviarlo
                });

                // Control de flujo (simulación)
                await Task.Delay(50); // Ajusta según el caso
            }

            // Notificar al cliente que el streaming ha finalizado
            await _hubContext.Clients.All.SendAsync("StreamEnded");
        }

        public async Task StreamVideoUrlBlob()
        {
            var filePath = "C:\\Users\\alvaroa\\Desktop\\videos\\Pelea_de_Monos.mp4";

            // Información del video
            var fileInfo = new FileInfo(filePath);
            var fileSize = fileInfo.Length;
            const int bufferSize = 64 * 1024; // 64 KB
            var buffer = new byte[bufferSize];

            // Enviar metainformación del video (opcional)
            await _hubContext.Clients.All.SendAsync("VideoInfo", new
            {
                FileName = fileInfo.Name,
                FileSize = fileSize,
                MimeType = "video/mp4"
            });

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            int bytesRead;
            while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                var chunk = buffer.Take(bytesRead).ToArray();

                // Enviar el chunk al cliente
                await _hubContext.Clients.All.SendAsync("ReceiveVideoChunkUrlBlob", chunk);

                // Control de flujo (simulación)
                await Task.Delay(50); // Ajusta según el caso
            }

            // Notificar al cliente que el streaming ha finalizado
            await _hubContext.Clients.All.SendAsync("StreamEnded");
        }

        public async Task StreamVideoFrames()
        {
            var filePath = "C:\\Users\\seleneo\\Desktop\\Videosa\\30s.mp4";

            using var videoCapture = new VideoCapture(filePath);
            if (!videoCapture.IsOpened())
            {
                Console.WriteLine("Error al abrir el video.");
                return;
            }

            var frameRate = (int)videoCapture.Fps; // Frames por segundo del video
            //var delay = frameRate; // Tiempo entre frames en ms
            var delay = 500 / frameRate; // Tiempo entre frames en ms

            using var mat = new Mat();
            while (true)
            {
                videoCapture.Read(mat);
                if (mat.Empty()) break; // No hay más frames, fin del video

                // Convierte el frame a un array de bytes (JPEG)
                var frameBytes = mat.ToBytes(".jpg");
                var frameBase64 = Convert.ToBase64String(frameBytes);

                // Enviar el frame al cliente
                await _hubContext.Clients.All.SendAsync("ReceiveFrame", frameBase64);

                // Simula el retraso entre frames
                await Task.Delay(delay);
            }

            await StreamVideoFrames();
            // Notificar al cliente que el streaming ha terminado
            //await _hubContext.Clients.All.SendAsync("StreamEnded");
        }

        public async Task StreamVideoFrames2()
        {
            var filePath = "C:\\Users\\seleneo\\Desktop\\Videosa\\5m.mp4";

            using var videoCapture = new VideoCapture(filePath);
            if (!videoCapture.IsOpened())
            {
                Console.WriteLine("Error al abrir el video.");
                return;
            }

            var frameRate = (int)videoCapture.Fps; // Frames por segundo del video
            //var delay = frameRate; // Tiempo entre frames en ms
            var delay = 500 / frameRate; // Tiempo entre frames en ms

            using var mat = new Mat();
            while (true)
            {
                videoCapture.Read(mat);
                if (mat.Empty()) break; // No hay más frames, fin del video

                // Convierte el frame a un array de bytes (JPEG)
                var frameBytes = mat.ToBytes(".jpg");
                var frameBase64 = Convert.ToBase64String(frameBytes);

                // Enviar el frame al cliente
                await _hubContext.Clients.All.SendAsync("ReceiveFrame2", frameBase64);

                // Simula el retraso entre frames
                await Task.Delay(delay);
            }

            await StreamVideoFrames2();
            // Notificar al cliente que el streaming ha terminado
            //await _hubContext.Clients.All.SendAsync("StreamEnded");
        }

        public async Task StreamVideoFrames3()
        {
            var filePath = "C:\\Users\\seleneo\\Desktop\\Videosa\\10m.mp4";

            using var videoCapture = new VideoCapture(filePath);
            if (!videoCapture.IsOpened())
            {
                Console.WriteLine("Error al abrir el video.");
                return;
            }

            var frameRate = (int)videoCapture.Fps; // Frames por segundo del video
            //var delay = frameRate; // Tiempo entre frames en ms
            var delay = 500 / frameRate; // Tiempo entre frames en ms

            using var mat = new Mat();
            while (true)
            {
                videoCapture.Read(mat);
                if (mat.Empty()) break; // No hay más frames, fin del video

                // Convierte el frame a un array de bytes (JPEG)
                var frameBytes = mat.ToBytes(".jpg");
                var frameBase64 = Convert.ToBase64String(frameBytes);

                // Enviar el frame al cliente
                await _hubContext.Clients.All.SendAsync("ReceiveFrame3", frameBase64);

                // Simula el retraso entre frames
                await Task.Delay(delay);
            }
            await StreamVideoFrames3();
            // Notificar al cliente que el streaming ha terminado
            //await _hubContext.Clients.All.SendAsync("StreamEnded");
        }

        public async Task StreamVideoFrames4()
        {
            var filePath = "C:\\Users\\seleneo\\Desktop\\Videosa\\1m.mp4";

            using var videoCapture = new VideoCapture(filePath);
            if (!videoCapture.IsOpened())
            {
                Console.WriteLine("Error al abrir el video.");
                return;
            }

            var frameRate = (int)videoCapture.Fps; // Frames por segundo del video
            //var delay = frameRate; // Tiempo entre frames en ms
            var delay = 500 / frameRate; // Tiempo entre frames en ms

            using var mat = new Mat();
            while (true)
            {
                videoCapture.Read(mat);
                if (mat.Empty()) break; // No hay más frames, fin del video

                // Convierte el frame a un array de bytes (JPEG)
                var frameBytes = mat.ToBytes(".jpg");
                var frameBase64 = Convert.ToBase64String(frameBytes);

                // Enviar el frame al cliente
                await _hubContext.Clients.All.SendAsync("ReceiveFrame4", frameBase64);

                // Simula el retraso entre frames
                await Task.Delay(delay);
            }
            await StreamVideoFrames4();
            // Notificar al cliente que el streaming ha terminado
            //await _hubContext.Clients.All.SendAsync("StreamEnded");
        }
        public async Task StreamVideoFrames5()
        {
            var filePath = "C:\\Users\\seleneo\\Desktop\\Videosa\\video1hs.mp4";

            using var videoCapture = new VideoCapture(filePath);
            if (!videoCapture.IsOpened())
            {
                Console.WriteLine("Error al abrir el video.");
                return;
            }

            var frameRate = (int)videoCapture.Fps; // Frames por segundo del video
            //var delay = frameRate; // Tiempo entre frames en ms
            var delay = 500 / frameRate; // Tiempo entre frames en ms

            using var mat = new Mat();
            while (true)
            {
                videoCapture.Read(mat);
                if (mat.Empty()) break; // No hay más frames, fin del video

                // Convierte el frame a un array de bytes (JPEG)
                var frameBytes = mat.ToBytes(".jpg");
                var frameBase64 = Convert.ToBase64String(frameBytes);

                // Enviar el frame al cliente
                await _hubContext.Clients.All.SendAsync("ReceiveFrame5", frameBase64);

                // Simula el retraso entre frames
                await Task.Delay(delay);
            }
            await StreamVideoFrames5();
            // Notificar al cliente que el streaming ha terminado
            //await _hubContext.Clients.All.SendAsync("StreamEnded");
        }

        public async Task StreamVideoFrames6()
        {
            var filePath = "C:\\Users\\seleneo\\Desktop\\Videosa\\video.mp4";

            using var videoCapture = new VideoCapture(filePath);
            if (!videoCapture.IsOpened())
            {
                Console.WriteLine("Error al abrir el video.");
                return;
            }

            var frameRate = (int)videoCapture.Fps; // Frames por segundo del video
            //var delay = frameRate; // Tiempo entre frames en ms
            var delay = 500 / frameRate; // Tiempo entre frames en ms

            using var mat = new Mat();
            while (true)
            {
                videoCapture.Read(mat);
                if (mat.Empty()) break; // No hay más frames, fin del video

                // Convierte el frame a un array de bytes (JPEG)
                var frameBytes = mat.ToBytes(".jpg");
                var frameBase64 = Convert.ToBase64String(frameBytes);

                // Enviar el frame al cliente
                await _hubContext.Clients.All.SendAsync("ReceiveFrame6", frameBase64);

                // Simula el retraso entre frames
                await Task.Delay(delay);
            }
            await StreamVideoFrames6();
            // Notificar al cliente que el streaming ha terminado
            //await _hubContext.Clients.All.SendAsync("StreamEnded");
        }

        public async Task InitAllFrames()
        {
            StreamVideoFrames();
            StreamVideoFrames2();
            StreamVideoFrames3();
            StreamVideoFrames4();
            StreamVideoFrames5();
            StreamVideoFrames6();

        }
    }
}
