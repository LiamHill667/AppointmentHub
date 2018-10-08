using FluentValidation;
using System;
using System.Globalization;

namespace AppointmentHub.Core.Validators
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> StringValidDateTimeFormat<T>(this IRuleBuilder<T, string> ruleBuilder, string format)
        {

            return ruleBuilder.Must((rootObject, dateString, context) =>
            {
                context.MessageFormatter.AppendArgument("Format", format);

                return DateTime.TryParseExact(dateString,
                        format,
                        CultureInfo.CurrentCulture,
                        DateTimeStyles.None,
                        out DateTime time) == true;
            })
            .WithMessage("{PropertyName} must be in the following format {Format}");
        }
    }
}