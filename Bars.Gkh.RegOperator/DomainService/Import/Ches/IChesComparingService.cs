namespace Bars.Gkh.RegOperator.DomainService.Import.Ches
{
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;

    /// <summary>
    /// Интерфейс для работы с сопоставлением данных во время импорта ЧЭС
    /// </summary>
    public interface IChesComparingService
    {
        /// <summary>
        /// Обработать добавленные адреса (метод записывает в таблицу несопоставленные)
        /// </summary>
        IDataResult ProcessAccountImported(IChesTempDataProvider chesTempDataProvider);

        /// <summary>
        /// Обработать добавление сопоставления
        /// </summary>
        IDataResult ProcessAddressMatchAdded(params AddressMatch[] address);

        /// <summary>
        /// Обработать удаление сопоставления
        /// </summary>
        IDataResult ProcessAddressMatchRemoved(AddressMatch address);

        /// <summary>
        /// Обработать добавление сопоставления абонента
        /// </summary>
        IDataResult ProcessOwnerMatchAdded(ChesMatchAccountOwner owner);

        /// <summary>
        /// Обработать удаление сопоставления абонента
        /// </summary>
        IDataResult ProcessOwnerMatchRemoved(ChesMatchAccountOwner owner);

        /// <summary>
        /// Сопоставить владельца
        /// </summary>
        IDataResult MatchOwner(BaseParams baseParams);
    }
}