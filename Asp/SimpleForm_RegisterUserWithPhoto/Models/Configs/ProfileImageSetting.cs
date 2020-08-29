using System.Collections.Generic;
using System.Text;

namespace SimpleForm_RegisterUserWithPhoto.Models.Configs
{
    public class ProfileImageSetting
    {
        private string[] _validTypes = null!;

        public string[] ValidTypes
        {
            get => _validTypes;
            set
            {
                _validTypes = value;
                ValidTypesStr = GetValidTypeStr(value);
            }
        }

        public string ValidTypesStr { get; private set; } = null!;

        private static string GetValidTypeStr(IReadOnlyCollection<string> validTypes)
        {
            var result = new StringBuilder(validTypes.Count * 5);
            foreach (var validType in validTypes)
                result.Append(".").Append(validType).Append(",");
            return result.ToString();
        }
    }
}