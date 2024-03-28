namespace Bars.Gkh.Modules.Reforma
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using System.Linq;

    /// <summary>
    /// Сервис получение "Протоколы собственников помещений МКД" 
    /// </summary>
    public interface IPropertyOwnerProtocolsProvider
    {
        /// <summary>
        /// Получение "Протоколы собственников помещений МКД" 
        /// </summary>
        /// <param name="realtyObjIdsQuery">Id жилога дома</param>
        /// <returns>Словарь из прокси объектов</returns>
        Dictionary<long, PropertyOwnerProtocolsData> GetData(IQueryable<long> realtyObjIdsQuery);
    }

    /// <summary>
    /// Прокси объект для "Протоколы собственников помещений МКД" 
    /// </summary>
    public class PropertyOwnerProtocolsData
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public FileInfo File { get; set; }
    }
}