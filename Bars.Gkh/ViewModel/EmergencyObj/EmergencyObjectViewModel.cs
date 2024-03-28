namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Entities;

    public class EmergencyObjectViewModel : BaseViewModel<EmergencyObject>
    {
        public override IDataResult List(IDomainService<EmergencyObject> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = this.Container.Resolve<IEmergencyObjectService>()
                .GetFilteredByOperator()
                .Select(x => new
                    {
                        x.Id,
                        x.State,
                        ResettlementProgramName = x.ResettlementProgram.Name,
                        Municipality = x.RealityObject.Municipality.Name,
                        Settlement = x.RealityObject.MoSettlement.Name,
                        x.RealityObject.FiasAddress.AddressName,
                        x.IsRepairExpedient,
                        x.ResettlementFlatAmount,
                        x.ResettlementFlatArea,
                        x.ConditionHouse,
                        x.FileInfo,
                        x.DocumentName,
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.Description

                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.AddressName)
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<EmergencyObject> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var obj = domainService.Get(id);

            var result = new
            {
                obj.Id,
                obj.RealityObject,
                obj.FurtherUse,
                obj.ReasonInexpedient,
                obj.ActualInfoDate,
                obj.CadastralNumber,
                obj.FileInfo,
                obj.DocumentName,
                obj.DocumentNumber,
                obj.DocumentDate,
                obj.Description,
                obj.EmergencyFileInfo,
                obj.EmergencyDocumentName,
                obj.EmergencyDocumentNumber,
                obj.EmergencyDocumentDate,
                obj.DemolitionDate,
                obj.ResettlementDate,
                obj.FactDemolitionDate,
                obj.FactResettlementDate,
                obj.LandArea,
                obj.ResettlementFlatArea,
                obj.ResettlementFlatAmount,
                obj.InhabitantNumber,
                obj.IsRepairExpedient,
                obj.ConditionHouse,
                obj.ResettlementProgram,
                obj.State,
                obj.ExemptionsBasis,
                obj.AddressName,
                TotalArea = obj.RealityObject.AreaLiving
            };

            return new BaseDataResult(result);
        }
    }
}