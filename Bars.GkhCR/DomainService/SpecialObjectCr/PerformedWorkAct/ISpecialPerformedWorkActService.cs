namespace Bars.GkhCr.DomainService
{
    using B4;

	/// <summary>
	/// Интерфейс для сервиса Акт выполненных работ
	/// </summary>
	public interface ISpecialPerformedWorkActService
    {
		/// <summary>
		/// Получить информацию по акту
		/// </summary>
		IDataResult GetInfo(BaseParams baseParams);

		/// <summary>
		/// Получить список актов по новым активным программам
		/// </summary>
		IDataResult ListByActiveNewOpenPrograms(BaseParams baseParams);
    }
}