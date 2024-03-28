namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
	/// <summary>
	/// Сервис для Определение
	/// </summary>
    public interface IDefinitionService
    {
		/// <summary>
		/// Получить максимальный номер определения
		/// </summary>
		/// <param name="year">Год</param>
		/// <returns>Номер определения</returns>
        int GetMaxDefinitionNum(int year);
    }
}