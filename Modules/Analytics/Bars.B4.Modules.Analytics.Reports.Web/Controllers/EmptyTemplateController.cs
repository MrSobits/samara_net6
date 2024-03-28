namespace Bars.B4.Modules.Analytics.Reports.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Serialization;
    using Bars.B4.Modules.Analytics.Utils;
    using Ionic.Zip;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер для примеров шаблонов
    /// </summary>
    public class EmptyTemplateController : BaseController
    {
        /// <summary>
        /// Скачать пример шаблона
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetEmptyTemplate(BaseParams baseParams)
        {
            Stream template;
            const string fileName = "Пример_шаблона";
            var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
            var addConn = baseParams.Params.GetAs("addConn", ignoreCase: true, defaultValue: false);
            var templateService = new EmptyTemplateService();
            var fileNameEncode = string.Empty;
            if (reportId > 0)
            {
                var reportDomain = Container.Resolve<IDomainService<StoredReport>>();
                var report = reportDomain.FirstOrDefault(x => x.Id == reportId);
                template = templateService.GetTemplateWithMeta(report.GetDataSources(), addConn, report.GetParams());
                Container.Release(reportDomain);

                template.Seek(0, SeekOrigin.Begin);

                fileNameEncode =
                    System.Web.HttpUtility.UrlEncode(string.Format("{0}_{1}.mrt", fileName, report != null ? report.Name : string.Empty));

                return File(template, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameEncode);
            }

            var dataSourcesIds = baseParams.Params.GetAs<string>("dataSourcesIds", ignoreCase: true).ToLongArray();
            var dataSourceDomain = Container.Resolve<IDomainService<DataSource>>();
            template =
                templateService.GetTemplateWithMeta(
                    dataSourceDomain.GetAll().Where(x => dataSourcesIds.Contains(x.Id)), addConn);
            Container.Release(dataSourceDomain);
            template.Seek(0, SeekOrigin.Begin);

            fileNameEncode =
                System.Web.HttpUtility.UrlEncode(string.Format("{0}.mrt", fileName));

            return File(template, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameEncode);
        }

        /// <summary>
        /// Скачать шаблон с тестовыми данными
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetTemplateWithSampleData(BaseParams baseParams)
        {
            var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
            var addConn = baseParams.Params.GetAs("addConn", ignoreCase: true, defaultValue: false);
            const string fileName = "Шаблон_с_тестовыми_данными";

            using (var zipFile = new ZipFile(Encoding.UTF8))
            {
                Stream template;
                var tplName = "Template";
                IEnumerable<IDataSource> dataSources;
                var templateService = new EmptyTemplateService();
                if (reportId > 0)
                {
                    var reportDomain = Container.Resolve<IDomainService<StoredReport>>();
                    var report = reportDomain.FirstOrDefault(x => x.Id == reportId);
                    template = templateService.GetTemplateWithMeta(report.GetDataSources(), addConn, report.GetParams());
                    tplName = report.Name;
                    dataSources = report.DataSources;
                    Container.Release(reportDomain);
                }
                else
                {
                    var dataSourcesIds =
                        baseParams.Params.GetAs<string>("dataSourcesIds", ignoreCase: true).ToLongArray();
                    var dataSourceDomain = Container.Resolve<IDomainService<DataSource>>();
                    dataSources = dataSourceDomain.GetAll().Where(x => dataSourcesIds.Contains(x.Id));
                    template =
                        templateService.GetTemplateWithMeta(dataSources, addConn);
                    Container.Release(dataSourceDomain);
                }

                template.Seek(0, SeekOrigin.Begin);
                zipFile.AddEntry(string.Format("{0}.mrt", tplName), template);
                var result = new MemoryStream();

                var serializer = new XmlSerializer();

                foreach (var dataSource in dataSources)
                {
                    var xelems = serializer.Serialize(dataSource.GetMetaData(), dataSource.GetSampleData());
                    foreach (var xElement in xelems)
                    {
                        var xml = string.Format("<?xml version=\"1.0\" encoding=\"UTF-8\"?>{0}", xElement);
                        var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                        stream.Seek(0, SeekOrigin.Begin);
                        zipFile.AddEntry(string.Format("{0}.xml", xElement.Name.LocalName), stream);
                    }
                }

                zipFile.Save(result);
                result.Seek(0, SeekOrigin.Begin);

                var fileNameEncode =
                    System.Web.HttpUtility.UrlEncode(string.Format("{0}_{1}.zip", fileName, tplName));

                return File(result, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameEncode);
            }
        }

    }
}
