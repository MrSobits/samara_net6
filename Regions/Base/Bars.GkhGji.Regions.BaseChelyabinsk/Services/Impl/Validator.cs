using System;
using System.Reflection;
using Bars.B4;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Services.Impl.Intfs;

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Services.Impl
{
    using Bars.B4.Utils;

    /// <summary>
    /// Валидатор
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Validator<TEntity> : IValidator<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Валидация
        /// </summary>
        public IDataResult Validate(PropertyInfo property, TEntity entity)
        {
            var attribute = Attribute.GetCustomAttribute(
                property,
                typeof(ValidationAttribute)) as ValidationAttribute;

            if (attribute == null)
            {
                return new BaseDataResult(true);
            }

            var value = property.GetValue(entity);

            switch (attribute.TypeData)
            {
                case TypeData.Text:
                    return this.ValidateText(attribute, value.ToStr());

                case TypeData.Number:
                    return this.ValidateNumber(attribute, (long?) value);

                case TypeData.Date:
                    return this.ValidateDate(attribute, (DateTime?) value);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Валидация текста
        /// </summary>
        /// <returns></returns>
        public IDataResult ValidateText(ValidationAttribute attribute, string value)
        {
            if (attribute.Required
                && string.IsNullOrEmpty(value))
                return new BaseDataResult(false, $"Поле '{attribute.Description}' не заполнено. {Environment.NewLine}");

            if (string.IsNullOrEmpty(value))
                return new BaseDataResult();

            if (attribute.MaxLength != default(int)
                && value.Length > attribute.MaxLength)
                return new BaseDataResult(
                    false,
                    $"Поле '{attribute.Description}' превышает длину (разрешено - {attribute.MaxLength}, длина поля - {value.Length}). {Environment.NewLine}");

            return new BaseDataResult();
        }

        /// <summary>
        /// Валидация числа
        /// </summary>
        /// <returns></returns>
        public IDataResult ValidateNumber(ValidationAttribute attribute, long? value)
        {
            if (attribute.Required)
            {
                if (value == null || value == default(long))
                    return new BaseDataResult(false, $"Поле '{attribute.Description}' не заполнено. {Environment.NewLine}");
            }

            if (attribute.MinValue != 0 && value < attribute.MinValue)
                return new BaseDataResult(
                    false,
                    $"Значение поля '{attribute.Description}' меньше разрешенного (разрешено - {attribute.MinValue}, значение поля - {value}). {Environment.NewLine}");

            if (attribute.MaxValue != 0 && value > attribute.MaxValue)
                return new BaseDataResult(
                    false,
                    $"Значение поля '{attribute.Description}' больше разрешенного (разрешено - {attribute.MaxValue}, значение поля - {value}). {Environment.NewLine}");

            return new BaseDataResult();
        }

        /// <summary>
        /// Валидация
        /// </summary>
        /// <returns></returns>
        public IDataResult ValidateDate(ValidationAttribute attribute, DateTime? value)
        {
            if (attribute.Required)
            {
                if (value == null || value == default(DateTime))
                    return new BaseDataResult(false, $"Поле '{attribute.Description}' не заполнено. {Environment.NewLine}");
            }

            return new BaseDataResult();
        }
    }

    /// <summary>
    /// Атрибут валидации
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidationAttribute : Attribute
    {
        /// <summary>
        /// Тип данных
        /// </summary>
        public TypeData TypeData { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Максимальная длина
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// Обязательность
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Минимальное значение
        /// </summary>
        public int MinValue { get; set; }

        /// <summary>
        /// Максимальное значение
        /// </summary>
        public int MaxValue { get; set; }
    }
}
