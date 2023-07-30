using IzmirBel.Survey.Services;
using Microsoft.AspNetCore.Mvc;
using nClam;

namespace IzmirBel.Survey.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FileUploadController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly FileValidationService _fileValidationService;
        private readonly long MaxFileSizeBytes = 0;
        private readonly string AntiVirusHost = string.Empty;
        private readonly int AntiVirusPort = 0;

        private const string UploadPath = "Uploads/";

        public FileUploadController(IConfiguration configuration, FileValidationService fileValidationService)
        {
            Configuration = configuration;
            MaxFileSizeBytes = Convert.ToInt64(Configuration["MaxFileSize"]);
            _fileValidationService = fileValidationService;
            AntiVirusHost = Configuration["AntiVirusHost:Host"];
            AntiVirusHost = Configuration["AntiVirusHost:Port"];
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Admin/FileUpload")]
        public async Task<IActionResult> FileUploadAsync(IFormFile uploadFile)
        {
            if (!ModelState.IsValid)
                return View("Index");

            if (uploadFile == null || uploadFile.Length == 0)
            {
                ModelState.AddModelError("File", "Invalid file");
                return View("Index");
            }

            if (uploadFile.Length > MaxFileSizeBytes)
            {
                ModelState.AddModelError("File", $"The file is too large, it can't be above {MaxFileSizeBytes} bytes");
                return View("Index");
            }

            if (!_fileValidationService.IsValid(uploadFile))
                {
                ModelState.AddModelError("File", $"The file type is invalid");
                return View("Index");
            }

            var scanResult = await VirusScan(uploadFile);

            switch (scanResult.Result)
            {
                case ClamScanResults.Clean:
                    await SaveFile(uploadFile);
                    break;
                case ClamScanResults.VirusDetected:
                    ModelState.AddModelError("File", $"The file contains a virus");
                    break;
                default:
                    ModelState.AddModelError("File", $"An error was occured while scanning the file");
                    break;
            }

            return View("Index");
        }

        public async Task SaveFile(IFormFile uploadFile)
        {
            var filePath = UploadPath + Path.GetRandomFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                await uploadFile.CopyToAsync(stream);
            }
        }

        private async Task<ClamScanResult> VirusScan(IFormFile uploadFile)
        {
            var clam = new ClamClient(AntiVirusHost, AntiVirusPort);

            var memoryStream = new MemoryStream();
            uploadFile.OpenReadStream().CopyTo(memoryStream);
            var fileBytes = memoryStream.ToArray();

            return await clam.SendAndScanFileAsync(fileBytes);
        }
    }
}
