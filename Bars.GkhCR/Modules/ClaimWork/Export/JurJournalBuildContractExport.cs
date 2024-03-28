namespace Bars.GkhCr.Modules.ClaimWork.Export
{
    using System.Collections;
    using B4;
    using B4.Modules.DataExport.Domain;
    using DomainService;

    /// <summary>
    /// 
    /// </summary>
    public class JurJournalBuildContractExport : BaseDataExportService
    {
        /// <summary>
        /// Метод получения Данных для Экспорта
        /// </summary>
        /// <param name="baseParams"/>
        /// <returns/>
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IJurJournalBuildContractService>();
            int totalCount;
            return service.GetList(baseParams, false, out totalCount);
        }
    }
}