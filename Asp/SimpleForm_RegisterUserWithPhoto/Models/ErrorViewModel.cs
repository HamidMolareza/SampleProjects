using System;

namespace SimpleForm_RegisterUserWithPhoto.Models {
    public class ErrorViewModel {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty (RequestId);
    }
}