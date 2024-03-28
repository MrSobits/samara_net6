namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    public class DecisionInspectionBaseViewModel : BaseViewModel<DecisionInspectionBase>
    {
        /// <summary>
        /// Получаем список объектов <see cref="DecisionInspectionBase"/>
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override IDataResult List(IDomainService<DecisionInspectionBase> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");

            return domainService
                .GetAll()
                .Where(x => x.Decision.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    InspectionBaseType = x.InspectionBaseType.Name,
                    x.OtherInspBaseType,
                    x.FoundationDate,
                    RiskIndicator = x.RiskIndicator != null ? x.RiskIndicator.Name : string.Empty
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}