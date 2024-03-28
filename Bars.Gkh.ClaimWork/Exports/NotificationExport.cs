namespace Bars.Gkh.ClaimWork.Exports
{
    using System.Collections;
    using B4;
    using B4.Modules.DataExport.Domain;
    using DomainService.Document;

    /// <summary>
    /// 
    /// </summary>
    public class NotificationExport : BaseDataExportService
    {
        /// <summary>
        /// Метод получения данных для экспорта
        /// </summary>
        /// <param name="baseParams"/>
        /// <returns/>
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<INotificationService>();
            int totalCount;
            return service.GetList(baseParams, false, out totalCount);
        }
    }
}