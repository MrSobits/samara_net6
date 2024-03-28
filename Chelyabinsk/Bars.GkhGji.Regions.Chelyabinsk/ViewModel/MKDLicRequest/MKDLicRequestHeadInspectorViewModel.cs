namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using System.Linq;

    public class MKDLicRequestHeadInspectorViewModel : BaseViewModel<MKDLicRequestHeadInspector>
    {
        public override IDataResult List(IDomainService<MKDLicRequestHeadInspector> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var requestId = baseParams.Params.GetAs<long>("mkdlicrequestId");

            var data = domainService.GetAll()
                .Where(x => x.MKDLicRequest.Id == requestId)
                .Select(x => new
                {
                    x.Id,
                    x.Inspector.Fio,
                    x.Inspector.Position,
                    x.Inspector.Phone,
                    x.Inspector.Email
                })
                .ToListDataResult(loadParams, this.Container);

            return data;
        }
    }
}