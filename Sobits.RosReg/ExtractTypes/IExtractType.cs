namespace Sobits.RosReg.ExtractTypes
{
    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Enums;

    public interface IExtractType
    {
        /// <summary>
        /// Код типа
        /// </summary>
        ExtractType Code { get; }

        /// <summary>
        /// Название типа
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Категория
        /// </summary>
        ExtractCategory Category { get; }

        /// <summary>
        /// Шаблон для идентификации типа
        /// </summary>
        string Pattern { get; }

        /// <summary>
        /// XSLT-трансформация для выписок
        /// </summary>
        string Xslt { get; }

        /// <summary>
        /// Функция обработки
        /// </summary>
        void Parse(Extract extract);

        /// <summary>
        /// Функция печати
        /// </summary>
        string Print(Extract extract);
    }
}