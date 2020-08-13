using System;
using System.ComponentModel.DataAnnotations;
using FunctionalUtility.Extensions;
using FunctionalUtility.ResultDetails;
using Microsoft.AspNetCore.Http;
using Server.Utility.Attributes;

namespace SimpleForm_RegisterUserWithPhoto.Utility.Attribute {

    [AttributeUsage (AttributeTargets.Property | AttributeTargets.Field |
        AttributeTargets.Parameter)]
    public class AgreementAttribute : ValidationAttribute {
        protected override ValidationResult IsValid (object? value,
                ValidationContext validationContext) =>
            value is null ?
            ValidationResult.Success :
            value!.As<bool> ()
            .OnSuccessFailWhen (agreement => agreement == false,
                new ErrorDetail (StatusCodes.Status400BadRequest))
            .MapToValidationResult (ErrorMessage);
    }
}