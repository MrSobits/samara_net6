namespace Bars.Gkh.MetaValueConstructor.DataFillers
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    /// <summary>
    /// Интерфейс сервиса по заполнению данными объектов-значений
    /// </summary>
    public interface IDataFillerService
    {
        /// <summary>
        /// Проставить значение одному объекту
        /// </summary>
        /// <param name="parameters">Параметры для извлечения данных</param>
        /// <param name="value">Объект-значение</param>
        void SetValue(BaseParams parameters, BaseDataValue value);

        /// <summary>
        /// Проставить значение всем объектам
        /// </summary>
        /// <param name="parameters">Параметры для извлечения данных</param>
        /// <param name="values">Объекты-значение</param>
        void SetValue(BaseParams parameters, IList<BaseDataValue> values);
    }
}