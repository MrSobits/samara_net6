namespace Bars.Gkh.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class EmergencyObjectDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            return Container.Resolve<IDomainService<EmergencyObject>>().GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    ResettlementProgramName = x.ResettlementProgram.Name,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.FiasAddress.AddressName,
                    x.IsRepairExpedient,
                    x.ResettlementFlatAmount,
                    x.ResettlementFlatArea,
                    x.ConditionHouse,
                    x.FileInfo 
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.AddressName)
                .Filter(loadParam, this.Container).Order(loadParam)
                .ToList();
        }
    }
}