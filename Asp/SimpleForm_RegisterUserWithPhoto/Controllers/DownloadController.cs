using System.IO;
using FunctionalUtility.Extensions;
using FunctionalUtility.ResultDetails.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SimpleForm_RegisterUserWithPhoto.Models.Configs;

namespace SimpleForm_RegisterUserWithPhoto.Controllers {
    [Route ("V1/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase {
        public const string ControllerName = "Download";
        private readonly string _filesPath;

        public DownloadController (SecureRootSetting secureRootSetting) {
            _filesPath = secureRootSetting.Path;
        }

        [HttpGet]
        public ActionResult Get (string id) {
            var fileStreamResult = Path.Combine (_filesPath, id)
                .Map (filePath => System.IO.File.Exists (filePath)
                    .FailWhen (isFileExist => !isFileExist,
                        new NotFoundError (message: "The file is not exist."))
                    .OnSuccess (() => System.IO.File.OpenRead (filePath))
                    .OnSuccess (fileStream => (fileStream, filePath))
                );

            if (!fileStreamResult.IsSuccess)
                return fileStreamResult.ReturnMethodResult ();
            var result = fileStreamResult.Value;

            var contentType = GetContentType (result.filePath);
            if (string.IsNullOrEmpty (contentType))
                return NotFound ();

            return File (result.fileStream, contentType, id);
        }

        private static string GetContentType (string fileName) => new FileExtensionContentTypeProvider ()
            .TryGetContentType (fileName, out var contentType)
            .Map (contentType);
    }
}