namespace Bars.Gkh.TechnicalPassport
{
    using Bars.B4;

    /// <summary>
    /// Поставщик данных технического паспорта
    /// </summary>
    public interface ITechnicalPassportDataProvider
    {
        /// <summary>
        /// Получить данные
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult GetData(BaseParams baseParams);

        /// <summary>
        /// Изменить данные
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult UpdateData(BaseParams baseParams);

        /// <summary>
        /// Удалить данные
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        IDataResult RemoveData(BaseParams baseParams);
    }
}
