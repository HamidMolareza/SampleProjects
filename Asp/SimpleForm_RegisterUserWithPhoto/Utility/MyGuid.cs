using System;

namespace SimpleForm_RegisterUserWithPhoto.Utility {
    public static class MyGuid {
        public static string Generate () =>
            Guid.NewGuid ().ToString ("N");
    }
}