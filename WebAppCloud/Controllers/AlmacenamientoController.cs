using Azure.Storage.Blobs;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;


namespace WebAppCloud.Controllers
{
    public class AlmacenamientoController : Controller
    {
        private readonly IConfiguration _configuration;

        public AlmacenamientoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SubirArchivo(IFormFile archivo)
        {
            if (archivo != null && archivo.Length > 0)
            {
                var connectionString = _configuration.GetConnectionString("AzureStorage");
                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient("archivos");
                var blobClient = containerClient.GetBlobClient(archivo.FileName);

                using (var stream = archivo.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                // Realizar otras acciones según sea necesario.
                return RedirectToAction("Inicio");
            }

            // Manejar el caso en que no se proporcionó ningún archivo.
            return View();
        }

    }
}
