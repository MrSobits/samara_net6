namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Resolution
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;
    using Bars.GkhGji.ViewModel;

    public class TatarstanProtocolMvdViewModel : ProtocolMvdViewModel<TatarstanProtocolMvd>
    {
        public override IDataResult Get(IDomainService<TatarstanProtocolMvd> domainService, BaseParams baseParams)
        {
                var intId = baseParams.Params.GetAs("id", 0L);
                var obj = domainService.GetAll().FirstOrDefault(x => x.Id == intId);

                obj.InspectionId = obj.Inspection.Id;
                obj.TimeOffense = obj.DateOffense.HasValue ? obj.DateOffense.Value.ToString("HH:mm") : string.Empty;
                obj.CitizenshipType = obj.CitizenshipType ?? CitizenshipType.RussianFederation;

                return new BaseDataResult(obj);
        }

        public override IDataResult List(IDomainService<TatarstanProtocolMvd> domainService, BaseParams baseParams)
        {
            var service = this.Container.Resolve<ITatarstanProtocolMvdService>();
            var loadParam = baseParams.GetLoadParam();
            var res = service.GetList(baseParams, false).ToListDataResult(loadParam, this.Container);
            this.Container.Release(service);

            return res;
        }
    }
}