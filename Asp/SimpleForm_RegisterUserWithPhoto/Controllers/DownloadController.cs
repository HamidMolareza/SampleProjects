using System.IO;
using FunctionalUtility.Extensions;
using FunctionalUtility.ResultDetails.Errors;
using Microsoft.AspNetCore.Mvc;
using SimpleForm_RegisterUserWithPhoto.Models.Configs;

namespace SimpleForm_RegisterUserWithPhoto.Controllers {
    [Route ("V1/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase {
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
                );

            if (!fileStreamResult.IsSuccess)
                return fileStreamResult.ReturnMethodResult ();
            return File (fileStreamResult.Value, "application/pdf", id);
        }
    }
}