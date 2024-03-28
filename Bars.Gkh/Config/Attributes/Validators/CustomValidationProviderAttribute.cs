namespace Bars.Gkh.Config.Attributes.Validators
{
    using System.ComponentModel.DataAnnotations;
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Utils.Annotations;

    /// <summary>
    /// Валидатор, проверяющий изменение значение свойства через IConfigValueValidator
    /// </summary>
    public class CustomValidationProviderAttribute : ValidationAttribute
    {
        private readonly string _validatorCode;

        /// <summary>
        /// .ctor
        /// </summary>
        public CustomValidationProviderAttribute(string validatorCode)
        {
            ArgumentChecker.NotNullOrEmptyOrWhitespace(validatorCode, "validatorCode");

            _validatorCode = validatorCode;
        }

        #region Overrides of ValidationAttribute

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult"/> class. 
        /// </returns>
        /// <param name="value">The value to validate.</param><param name="validationContext">The context information about the validation operation.</param>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var container = ApplicationContext.Current.Container;

            if (container.HasComponent(_validatorCode))
            {
                var validator = container.Resolve<IConfigValueValidator>(_validatorCode);
                using (container.Using(validator))
                {
                    return validator.Validate(value, validationContext);
                }
            }

            return base.IsValid(value, validationContext);
        }

        #endregion
    }
}