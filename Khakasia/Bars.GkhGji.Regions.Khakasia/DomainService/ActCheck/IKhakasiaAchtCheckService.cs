namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
    using Bars.B4;

	/// <summary>
	/// Сервис для Акт проверки
	/// </summary>
    public interface IKhakasiaActCheckService
    {
		/// <summary>
		/// Получить список жилых домов для акта проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		IDataResult ListRealObjForActCheck(BaseParams baseParams);
    }
}