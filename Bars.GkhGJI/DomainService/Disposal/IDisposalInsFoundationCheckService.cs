namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IDisposalInsFoundationCheckService
    {
        /// <summary>
        /// Добавить "НПА проверки"
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult AddInspFoundationChecks(BaseParams baseParams);

        /// <summary>
        /// Добавить "НПА проверки"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="ids">Идентификаторы новых записей</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult AddInspFoundationChecks(long documentId, long[] ids);

        /// <summary>
        /// Добавить Требования НПА проверки
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult AddNormDocItems(BaseParams baseParams);
	}
}