namespace Bars.GkhGji.Regions.Tula.DomainService.Inspection.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class BaseStatementService : GkhGji.DomainService.BaseStatementService
    {
        protected override void GetAppealCitizenInfo(ref string appealCitizensNames, ref string appealCitizensIds, long inspectionId)
        {
            // Пробегаемся по обращениям и формируем итоговую строку наименований и строку идентификаторов
            var dataInspectors =
                Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                    .Where(x => x.Inspection.Id == inspectionId)
                    .Select(x => new { x.AppealCits.Id, x.AppealCits.NumberGji })
                    .ToList();

            appealCitizensNames = string.Join(", ", dataInspectors.Select(x => x.NumberGji));
            appealCitizensIds = string.Join(", ", dataInspectors.Select(x => x.Id.ToStr()));
        }
    }
}