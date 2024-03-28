namespace Bars.Gkh.Overhaul.Export
{
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Overhaul.DomainService;

    using Entities;

    // Пустышка на случай если гдето наследовались
    public class WorkPriceDataExport : WorkPriceDataExport<WorkPrice>
    {
        // Внимание методы ппереопределят ьи добавлять в Generic класс
    }
    
    // Generic для WorkPrice чтобы легче расширять сущность WorkPrice В регионах
    public class WorkPriceDataExport<T> : BaseDataExportService
        where T : WorkPrice
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IWorkPriceService<WorkPrice>>();

            try
            {
                int totalCount = 0;
                return service.GetListView(baseParams, false, ref totalCount);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}
