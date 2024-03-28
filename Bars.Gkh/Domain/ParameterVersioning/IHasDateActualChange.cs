namespace Bars.Gkh.Domain.ParameterVersioning
{
    using System;

    /// <summary>
    /// Интерфейс для указания даты актуального изменения параметра версионирования
    /// </summary>
    public interface IHasDateActualChange
    {
        /// <summary>
        /// Дата вступления параметра в силу
        /// </summary>
        DateTime ActualChangeDate { get; }
    }
}