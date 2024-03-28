namespace Bars.Gkh.MetaValueConstructor.DataFillers
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    /// <summary>
    /// Интерфейс поставщика работы с регистрациями источников данных
    /// </summary>
    public interface IDataFillerProvider
    {
        /// <summary>
        /// Вернуть описание всех источников
        /// </summary>
        /// <returns>Перечисление</returns>
        IEnumerable<DataFillerInfo> GetDataFillerInfo();

        /// <summary>
        /// Инициализировать
        /// </summary>
        void Init();

        /// <summary>
        /// Вернуть дерево источников данных
        /// </summary>
        /// <param name="baseParams"> Параметры запроса </param>
        /// <returns> Дерево </returns>
        DataTree<DataFillerInfo> GetTree(BaseParams baseParams);
    }
}