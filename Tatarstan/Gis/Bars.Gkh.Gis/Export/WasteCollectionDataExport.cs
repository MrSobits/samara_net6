namespace Bars.Gkh.Gis.Export
{
    using System.Collections;
    using System.Linq;

    using B4;
    using B4.Modules.DataExport.Domain;
    using B4.Utils;
    using Entities.WasteCollection;

    public class WasteCollectionDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            return Container.Resolve<IDomainService<WasteCollectionPlace>>().GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    Settlement = x.RealityObject.MoSettlement.Name,
                    x.RealityObject.Address,
                    Customer = x.Customer.Name,
                    x.TypeWaste,
                    x.TypeWasteCollectionPlace,
                    x.ContainersCount,
                    x.LandfillDistance,
                    Contractor = x.Contractor.Name,
                    x.NumberContract
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Settlement)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}