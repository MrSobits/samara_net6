namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities.PersonalAccount;

	/// <summary>
	/// Контроллер для Информация по начисленным льготам
	/// </summary>
	public class PersonalAccountBenefitsController : B4.Alt.DataController<PersonalAccountBenefits>
    {
		/// <summary>
		/// Сменить значение Сумма
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult UpdateSum(BaseParams baseParams)
        {
            var result = this.Resolve<IPersonalAccountBenefitsService>().UpdateSum(baseParams);
            return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
        }
    }
}