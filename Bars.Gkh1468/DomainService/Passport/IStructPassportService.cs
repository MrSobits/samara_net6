namespace Bars.Gkh1468.DomainService.Passport
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс для взаимодействия с разделами структур паспортов
    /// </summary>
    public interface IStructPartService
    {
        /// <summary>
        /// Удаляет переданный раздел
        /// </summary>
        /// <param name="id">Идентификатор раздела</param>
        /// <returns>Результат операции</returns>
        IDataResult RemovePart(long id);
    }
}
