namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using Entities;

	/// <summary>
	/// Интерфейс для сервиса Акт выполненных работ
	/// </summary>
	public interface IPerformedWorkActService
    {
		/// <summary>
		/// Получить список актов
		/// </summary>
		IDataResult ListAct(BaseParams baseParams);

		/// <summary>
		/// Получить информацию по акту
		/// </summary>
		IDataResult GetInfo(BaseParams baseParams);

		/// <summary>
		/// Получить отфильтрованный запрос по оператору
		/// </summary>
        IQueryable<PerformedWorkAct> GetFilteredByOperator();

		/// <summary>
		/// Получить список актов по новым активным программам
		/// </summary>
		IDataResult ListByActiveNewOpenPrograms(BaseParams baseParams);

		/// <summary>
		/// Получить сводную информацию по актам
		/// </summary>
		IDataResult ListDetails(BaseParams baseParams);

		/// <summary>
		/// Проверить допустимые акты для сводной информации
		/// </summary>
		IDataResult CheckActsForDetails(BaseParams baseParams);
    }
}