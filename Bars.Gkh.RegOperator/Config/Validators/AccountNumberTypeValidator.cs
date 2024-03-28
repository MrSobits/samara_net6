namespace Bars.Gkh.RegOperator.Config.Validators
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Impl.Internal;
    using Bars.Gkh.RegOperator.Entities;

    public class AccountNumberTypeValidator : IConfigValueValidator
    {
        private readonly IDomainService<BasePersonalAccount> _domain;

        public static readonly string Id = "AccountNumberTypeValidator";

        public AccountNumberTypeValidator(IDomainService<BasePersonalAccount> domain)
        {
            _domain = domain;
        }

        #region Implementation of IConfigValueValidator

        /// <summary>
        /// Провалидировать изменение свойства. <br/>
        /// Интерфейс аналогичен ValidationAttribute
        /// </summary>
        /// <param name="value">Новое значение</param>
        /// <param name="ctx">Контекст валидации</param>
        public ValidationResult Validate(object value, ValidationContext ctx)
        {
            var currentValue = ctx.ObjectInstance.As<ValueHolder>().Value;

            if (!value.Equals(currentValue) && _domain.GetAll().Any())
            {
                return new ValidationResult("Невозможно сменить способ генерации лицевых счетов, так как существуют сформированные лицевые счета");
            }

            return ValidationResult.Success;
        }

        #endregion
    }
}