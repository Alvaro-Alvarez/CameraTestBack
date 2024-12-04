using KafkaTest.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KafkaTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly VideoStreamService _videoStreamService;

        public TestController(VideoStreamService videoStreamService)
        {
            _videoStreamService = videoStreamService;
        }

        [HttpGet("StreamVideoMediaSource")]
        public async Task<ActionResult> StreamVideoMediaSource()
        {
            await _videoStreamService.StreamVideoMediaSource();
            return Ok();
        }

        [HttpGet("StreamVideoUrlBlob")]
        public async Task<ActionResult> StreamVideoUrlBlob()
        {
            await _videoStreamService.StreamVideoUrlBlob();
            return Ok();
        }

        [HttpGet("StreamVideoFrames")]
        public async Task<ActionResult> StreamVideoFrames()
        {
            await _videoStreamService.StreamVideoFrames();
            return Ok();
        }

        [HttpGet("InitAllFrames")]
        public async Task<ActionResult> InitAllFrames()
        {
            await _videoStreamService.InitAllFrames();
            return Ok();
        }

        [HttpGet("OnlyApi")]
        public async Task<ActionResult> OnlyApi()
        {
            var BufferSize = 1024 * 1024;
            var filePath = "C:\\Users\\alvaroa\\Desktop\\videos\\videolargo.mp4";
            //var filePath = "C:\\Users\\alvaroa\\Desktop\\videos\\Pelea_de_Monos.mp4";

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound("El archivo no existe.");
            }

            var fileInfo = new FileInfo(filePath);
            long totalLength = fileInfo.Length;

            // Configuración del rango
            Request.Headers.TryGetValue("Range", out var rangeHeader);
            long start = 0, end = totalLength - 1;

            if (!string.IsNullOrEmpty(rangeHeader))
            {
                var range = rangeHeader.ToString().Replace("bytes=", "").Split('-');
                start = long.Parse(range[0]);
                if (range.Length > 1 && !string.IsNullOrEmpty(range[1]))
                {
                    end = long.Parse(range[1]);
                }
            }

            if (start >= totalLength || end >= totalLength || start > end)
            {
                return StatusCode(416, "Rango no válido.");
            }

            long contentLength = end - start + 1;

            Response.Headers.Add("Accept-Ranges", "bytes");
            Response.Headers.Add("Content-Length", contentLength.ToString());
            Response.Headers.Add("Content-Range", $"bytes {start}-{end}/{totalLength}");

            Response.ContentType = "video/mp4";

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileStream.Seek(start, SeekOrigin.Begin);

            byte[] buffer = new byte[BufferSize];
            long remainingBytes = contentLength;

            while (remainingBytes > 0)
            {
                int bytesToRead = (int)Math.Min(BufferSize, remainingBytes);
                int bytesRead = await fileStream.ReadAsync(buffer, 0, bytesToRead);

                if (bytesRead == 0) break;

                await Response.Body.WriteAsync(buffer, 0, bytesRead);
                remainingBytes -= bytesRead;
            }

            return new EmptyResult();
        }



        [HttpGet("streamRange")]
        public IActionResult StreamVideo()
        {
            // Aquí simulas obtener información del archivo basado en el ID (normalmente desde la base de datos).
            var fileName = "sonic.mp4"; // Implementa esta lógica.
            var filePath = Path.Combine(@"C:\Users\alvaroa\Desktop\videos", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("El archivo no existe.");
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "video/mp4", enableRangeProcessing: true);
        }

        [HttpGet("streamRange2")]
        public IActionResult StreamVideo2()
        {
            // Aquí simulas obtener información del archivo basado en el ID (normalmente desde la base de datos).
            var fileName = "videocorto.mp4"; // Implementa esta lógica.
            var filePath = Path.Combine(@"C:\Users\alvaroa\Desktop\videos", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("El archivo no existe.");
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "video/mp4", enableRangeProcessing: true);
        }

        [HttpGet("streamRange3")]
        public IActionResult StreamVideo3()
        {
            // Aquí simulas obtener información del archivo basado en el ID (normalmente desde la base de datos).
            var fileName = "Pelea_de_Monos.mp4"; // Implementa esta lógica.
            var filePath = Path.Combine(@"C:\Users\alvaroa\Desktop\videos", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("El archivo no existe.");
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "video/mp4", enableRangeProcessing: true);
        }
    }
}
