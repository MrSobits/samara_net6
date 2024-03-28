namespace Bars.GkhGji.Controllers.Dict
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

	/// <summary>
	/// Контроллер для "Типы обследования"
	/// </summary>
    public class TypeSurveyGjiController : B4.Alt.DataController<TypeSurveyGji>
    {
        /// <summary>
        ///  Метод добавления записей административных регламентов типа обследования
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult AddAdministrativeRegulations(BaseParams baseParams)
        {
            return Resolve<ITypeSurveyGjiService>().AddAdministrativeRegulations(baseParams).ToJsonResult();
        }

		/// <summary>
		///     Метод добавления записей видов проверок типа обследования
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult AddKindInsp(BaseParams baseParams)
        {
            return Resolve<ITypeSurveyGjiService>().AddKindInsp(baseParams).ToJsonResult();
        }

		/// <summary>
		///     Метод добавления записей предоставляемых документов типа обследования
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult AddProvidedDocuments(BaseParams baseParams)
        {
            return Resolve<ITypeSurveyGjiService>().AddProvidedDocuments(baseParams).ToJsonResult();
        }

		/// <summary>
		///     Метод добавления записей целей проверка типа обследования
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult AddGoals(BaseParams baseParams)
        {
            return Resolve<ITypeSurveyGjiService>().AddGoals(baseParams).ToJsonResult();
        }

		/// <summary>
		///     Метод добавления записей задач проверки типа обследования
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult AddTaskInsp(BaseParams baseParams)
        {
            return Resolve<ITypeSurveyGjiService>().AddTaskInsp(baseParams).ToJsonResult();
        }

		/// <summary>
		///     Метод добавления записей правовых оснований типа обследования
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult AddInspFoundation(BaseParams baseParams)
        {
            return Resolve<ITypeSurveyGjiService>().AddInspFoundation(baseParams).ToJsonResult();
        }

		/// <summary>
		///     Метод добавления записей НПА проверки
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public ActionResult AddInspFoundationChecks(BaseParams baseParams)
		{
			return Resolve<ITypeSurveyGjiService>().AddInspFoundationChecks(baseParams).ToJsonResult();
		}
	}
}