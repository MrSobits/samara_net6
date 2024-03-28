namespace Bars.B4.Modules.Analytics.Reports.Domain
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;

    /// <summary>
    /// 
    /// </summary>
    public class EmptyTemplateService
    {
        /// <summary>
        /// Пустой mrt-шаблон.
        /// </summary>
        public static byte[] EmptyTemplate
        {
            get { return Properties.Resources.EmptyReport; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataSources"></param>
        /// <param name="addConn"></param>
        /// <param name="paramsList"></param>
        /// <returns></returns>
        public Stream GetTemplateWithMeta(IEnumerable<IDataSource> dataSources, bool addConn = false, IEnumerable<IParam> paramsList = null)
        {
            var emptyReportStream = new MemoryStream(Properties.Resources.EmptyReport);

            var report = new CustomReport(dataSources,
                paramsList ?? Array.Empty<IParam>(),
                this.GetType().Name,
                "EmptyReport",
                emptyReportStream);

            var extraParams = new Dictionary<string, object>
            {
                { "ConnectionString", ApplicationContext.Current.Configuration.ConnString }
            };

            var container = ApplicationContext.Current.Container;
            var remoteReportService = container.Resolve<IRemoteReportService>();

            using (container.Using(remoteReportService))
            {
                return remoteReportService.GetTemplateWithMeta(report,
                    new BaseParams(),
                    extraParams);   
            }
        }
    }
}
