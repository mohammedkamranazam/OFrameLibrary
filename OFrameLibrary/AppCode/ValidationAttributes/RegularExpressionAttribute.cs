using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace OFrameLibrary.ValidationAttributes
{
    public class UniRegularExpressionAttribute : ValidationAttribute, IClientValidatable
    {
        string regularExpressionType;
        readonly string message;
        readonly string expression;

        public UniRegularExpressionAttribute(string regularExpressionType)
        {
            this.regularExpressionType = regularExpressionType;

            expression = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            message = "Regular Expression Invalid Message. DisplayName [{0}]";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (Regex.IsMatch(value.ToString(), expression, RegexOptions.IgnoreCase))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(message);
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            bool required = metadata.IsRequired;

            var errorMessage = string.Format(message, metadata.DisplayName);

            var clientRule = new ModelClientValidationRule();
            clientRule.ErrorMessage = errorMessage;
            clientRule.ValidationType = "uniregexp";
            clientRule.ValidationParameters.Add("regex", expression);
            clientRule.ValidationParameters.Add("required", required);

            yield return clientRule;
        }
    }
}