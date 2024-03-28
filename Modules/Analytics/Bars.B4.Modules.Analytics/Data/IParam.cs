namespace Bars.B4.Modules.Analytics.Data
{
    using System;
    using Bars.B4.Modules.Analytics.Enums;

    /// <summary>
    /// Интерфейс параметра.
    /// </summary>
    public interface IParam
    {
        /// <summary>
        /// Способ создания.
        /// </summary>
        OwnerType OwnerType { get;}

        /// <summary>
        /// Тип параметра.
        /// </summary>
        ParamType ParamType { get; }

        /// <summary>
        /// Обязательность параметра.
        /// </summary>
        bool Required { get; }

        /// <summary>
        /// Множественный выбор
        /// </summary>
        bool Multiselect { get; }

        /// <summary>
        /// Текст, используемый для показа пользователю.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Наименование параметра.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Sql-запрос
        /// </summary>
        string SqlQuery { get; }

        /// <summary>
        /// Поле, содержащее доп. информацию для формирование киентского field
        /// Например, при <see cref="ParamType"/> = <see cref="Bars.B4.Modules.Analytics.Enums.ParamType"/>,
        /// хранит идентификатор зарегистрированного справочника. 
        /// </summary>
        string Additional { get; }

        /// <summary>
        /// Значение по умолчанию.
        /// </summary>
        object DefaultValue { get; }

        /// <summary>
        /// 
        /// </summary>
        Type CLRType { get; }
    }
}
