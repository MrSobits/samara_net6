namespace Bars.Gkh.Config
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Интерфейс валидации свойства конфигурации
    /// </summary>
    public interface IConfigValueValidator
    {
        /// <summary>
        /// Провалидировать изменение свойства. <br/>
        /// Интерфейс аналогичен ValidationAttribute
        /// </summary>
        /// <param name="value">Новое значение</param>
        /// <param name="ctx">Контекст валидации</param>
        ValidationResult Validate(object value, ValidationContext ctx);
    }
}