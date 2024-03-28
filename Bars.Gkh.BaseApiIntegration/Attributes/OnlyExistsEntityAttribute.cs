namespace Bars.Gkh.BaseApiIntegration.Attributes
{
    using System;

    /// <summary>
    /// Атрибут с проверкой наличия
    /// сущности(-ей) с переданным(-и) идентификатором(-ами)
    /// </summary>
    /// <remarks>
    /// Проверка накладывается ТОЛЬКО при заполнении свойства
    /// </remarks>
    public class OnlyExistsEntityAttribute : RequiredExistsEntityAttribute
    {
        /// <inheritdoc />
        public OnlyExistsEntityAttribute(Type entityType)
            : base(entityType)
        {
        }

        /// <inheritdoc />
        public override bool IsValid(object objValue)
        {
            // Корректность заполнения свойства
            var result = this.RequiredCheck(objValue);

            // Наложить проверку наличия сущности ТОЛЬКО при правильном заполнении
            return !result || this.ConvertValueAndExistsCheck(objValue);
        }
    }
}