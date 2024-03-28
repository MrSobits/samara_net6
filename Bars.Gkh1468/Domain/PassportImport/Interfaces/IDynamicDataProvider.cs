namespace Bars.Gkh1468.Domain.PassportImport.Interfaces
{
    /// <summary>
    /// Провайдер данных для разбора и сохранения паспорта
    /// </summary>
    public interface IDynamicDataProvider
    {
        /// <summary>
        /// Получить данные для разбора 
        /// </summary>
        /// <returns>Данные в произвольном формате. Клиент сам знает в каком формате нужны данные</returns>
        object GetData();
    }
}