namespace Bars.GkhGji.Regions.Khakasia.DomainService.Inspection.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

	/// <summary>
	/// Сервис для работы с основаниями обращениями граждан
	/// </summary>
	public class BaseStatementService : GkhGji.DomainService.BaseStatementService
    {
		/// <summary>
		/// Метод получения информации об обращениях граждан
		/// </summary>
		/// <param name="appealCitizensNames">Имена граждан</param>
		/// <param name="appealCitizensIds">Идентификаторы граждан</param>
		/// <param name="inspectionId">Идентификатор проверки ГЖИ</param>
		protected override void GetAppealCitizenInfo(ref string appealCitizensNames, ref string appealCitizensIds, long inspectionId)
        {
            // Пробегаемся по обращениям и формируем итоговую строку наименований и строку идентификаторов
            var dataInspectors =
                this.Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                    .Where(x => x.Inspection.Id == inspectionId)
                    .Select(x => new { x.AppealCits.Id, x.AppealCits.NumberGji })
                    .ToList();

            appealCitizensNames = string.Join(", ", dataInspectors.Select(x => x.NumberGji));
            appealCitizensIds = string.Join(", ", dataInspectors.Select(x => x.Id.ToStr()));
        }
    }
}