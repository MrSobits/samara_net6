namespace Bars.Gkh.Controllers.Dict
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.Dict;
    using Bars.Gkh.Entities.Dicts;

	/// <summary>
	/// Контроллер для <see cref="ResettlementProgram"/>
	/// </summary>
	public class ResettlementProgramController : B4.Alt.DataController<ResettlementProgram>
    {
		/// <summary>
		/// Сервис для <see cref="ResettlementProgram"/>
		/// </summary>
		public IResettlementProgramService ResettlementProgramService { get; set; }

		/// <summary>
		/// Получить список программ переселения без пагинации
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult ListWithoutPaging(BaseParams baseParams)
		{
			var result = this.ResettlementProgramService.ListWithoutPaging(baseParams);
			return result.ToJsonResult();
		}
    }
}
