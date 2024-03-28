namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    public class AppealCitsHeadInspectorViewModel: BaseViewModel<AppealCitsHeadInspector>
    {
        public override IDataResult List(IDomainService<AppealCitsHeadInspector> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var appealCitizensId = loadParams.Filter.GetAsId("appealCitizensId");

            return domainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}