namespace Bars.Gkh.DomainService.Dict
{
    using Bars.B4;
    using Entities.Dicts;

    /// <summary>
    /// Интерфейс сервиса для <see cref="ResettlementProgram"/>
    /// </summary>
    public interface IResettlementProgramService
	{
        /// <summary>
        /// Получить список программ переселения без пагинации
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult ListWithoutPaging(BaseParams baseParams);
    }
}