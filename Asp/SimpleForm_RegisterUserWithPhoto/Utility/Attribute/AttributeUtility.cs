using System.ComponentModel.DataAnnotations;
using FunctionalUtility.Extensions;
using FunctionalUtility.ResultUtility;

namespace Server.Utility.Attributes {
    public static class AttributeUtility {
        private static string GetErrorMessage (ResultDetail resultDetail, string? userErrorMessage) =>
            !string.IsNullOrEmpty (userErrorMessage) ?
            userErrorMessage :
            resultDetail.Message ?? "{0} is not valid.";

        public static ValidationResult MapToValidationResult<T> (
            this MethodResult<T> @this,
            string? userErrorMessage
        ) => @this.MapMethodResult (
            _ => ValidationResult.Success,
            errorDetail =>
            new ValidationResult (GetErrorMessage (errorDetail, userErrorMessage))
        );

        public static ValidationResult MapToValidationResult (
            this MethodResult @this,
            string? userErrorMessage
        ) => @this.MapMethodResult (
            () => ValidationResult.Success,
            errorDetail =>
            new ValidationResult (GetErrorMessage (errorDetail, userErrorMessage))
        );
    }
}