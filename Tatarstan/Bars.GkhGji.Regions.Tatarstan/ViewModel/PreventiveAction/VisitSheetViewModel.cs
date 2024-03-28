namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class VisitSheetViewModel : BaseViewModel<VisitSheet>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<VisitSheet> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var hasViolThreats = false;
            
            var visitSheetViolationInfoDomain = this.Container.ResolveDomain<VisitSheetViolationInfo>();
            var visitSheetViolationDomain = this.Container.ResolveDomain<VisitSheetViolation>();

            using (this.Container.Using(visitSheetViolationDomain, visitSheetViolationInfoDomain))
            {
                var entity = domainService.Get(id);
                if (entity == null)
                {
                    return base.Get(domainService, baseParams);
                }
            
                // Получаем нарушения по документу
                var violationIds = visitSheetViolationInfoDomain
                    .GetAll()
                    .Where(x => x.VisitSheet.Id == id)
                    .Select(x => x.Id)
                    .ToList();

                if (violationIds.Any())
                {
                    // Проверяем есть хоть одна угроза в нарушениях у документа
                    hasViolThreats = visitSheetViolationDomain
                        .GetAll()
                        .Where(x => violationIds.Contains(x.ViolationInfo.Id))
                        .Any(x => x.IsThreatToLegalProtectedValues);
                }
                
                return new BaseDataResult(new
                {
                    entity.Id,
                    entity.DocumentDate,
                    entity.DocumentYear,
                    entity.DocumentNumber,
                    entity.DocumentNum,
                    entity.DocumentSubNum,
                    entity.ExecutingInspector,
                    entity.HasCopy,
                    entity.VisitDateStart,
                    entity.VisitDateEnd,
                    entity.TypeDocumentGji,
                    entity.Stage,
                    entity.State,
                    VisitTimeStart = entity.VisitTimeStart.HasValue ? entity.VisitTimeStart.Value.ToString("HH:mm") : "",
                    VisitTimeEnd = entity.VisitTimeEnd.HasValue ? entity.VisitTimeEnd.Value.ToString("HH:mm") : "",
                    HasViolThreats = hasViolThreats
                });
            }
        }
    }
}