namespace Bars.GkhGji.Regions.Nnovgorod.DomainService.Disposal.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DisposalService : GkhGji.DomainService.DisposalService
    {
        protected override void GetInfoCitizenStatement(ref string baseName, ref string planName, long inspectionId, BaseStatementRequestType? requestType)
        {
            // распоряжение создано на основе обращения граждан,
            // поле planName = "Обращение № Номер обращения"

            baseName = "Обращение граждан";

            // Получаем из основания наименование плана
            var baseStatement = string.Join(
                ", ",
                Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                    .Where(x => x.Inspection.Id == inspectionId)
                    .Select(x => x.AppealCits.DocumentNumber));

            if (!string.IsNullOrWhiteSpace(baseStatement))
            {
                planName = string.Format("Обращение № ({0})", baseStatement);
            }
        }
    }
}