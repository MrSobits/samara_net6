namespace Bars.Gkh.DomainService.AddressMatching
{
    using System.Collections.Generic;
    using Bars.Gkh.Dto;

    /// <summary>
    /// Интерфейс для сервиса сохранения несопоставленных домов при импорте
    /// </summary>
    public interface IImportedAddressService
    {
        /// <summary>
        /// Метод для сохранения несопоставленных адресов
        /// </summary>
        /// <param name="addresses">Список адресов</param>
        /// <param name="filename">Имя файла импорта</param>
        /// <param name="importType">Тип импорта</param>
        void Save(List<NotMatchedAddressInfo> addresses, string filename, string importType);
    }
}
