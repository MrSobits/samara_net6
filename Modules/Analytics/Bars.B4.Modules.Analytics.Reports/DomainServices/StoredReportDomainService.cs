namespace Bars.B4.Modules.Analytics.Reports.DomainServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Utils;
    using Bars.B4.Utils;
    using CollectionExtensions = Castle.Core.Internal.CollectionExtensions;
	

    /// <summary>
    /// Домен-сервис для хранимых отчётов
    /// </summary>
    public class StoredReportDomainService : BaseDomainService<StoredReport>
    {
        /// <summary>
        /// Метод сохранить
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат сохранения</returns>
        public override IDataResult Save(BaseParams baseParams)
        {
            IDataResult result = null;
            this.InTransaction(() =>
            {
                result = base.Save(baseParams);
                var reports = result.Data as List<StoredReport>;
                if (reports != null && reports.Count == 1)
                {
                    this.SaveDataSources(baseParams, reports.First());
                    this.SaveReportParams(baseParams, reports.First());
                    this.SaveTemplate(baseParams, reports.First());
                }
            });
            return result;
        }

        /// <summary>
        /// Метод обновить
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат обновления</returns>
        public override IDataResult Update(BaseParams baseParams)
        {
            IDataResult result = null;
            this.InTransaction(() =>
            {
                result = base.Update(baseParams);
                var reports = result.Data as List<StoredReport>;
                if (reports != null && reports.Count == 1)
                {
                    this.SaveReportParams(baseParams, reports.First());
                    this.SaveDataSources(baseParams, reports.First());
                    this.SaveTemplate(baseParams, reports.First());
                }
            });
            return result;
        }

        /// <summary>
        /// Внутренний метод удаления
        /// </summary>
        /// <param name="id">Идентификатор отчёта</param>
        protected override void DeleteInternal(object id)
        {
            var reportParamDomain = this.Container.Resolve<IDomainService<ReportParamGkh>>();
            using (this.Container.Using(reportParamDomain))
            {
                var paramsToDelete = reportParamDomain.GetAll().Where(x => x.StoredReport.Id == id.ToLong()).ToList();
                foreach (var reportParam in paramsToDelete)
                {
                    reportParamDomain.Delete(reportParam.Id);
                }
                base.DeleteInternal(id);
            }
        }

        private void SaveTemplate(BaseParams baseParams, StoredReport report)
        {
            if (baseParams.Files.Any())
            {
                report.AddTemplate(baseParams.Files["Template"]);
            }
            else
            {
                report.SetTemplate(Convert.FromBase64String(baseParams.Params.GetAs<string>("TemplateFile")));
            }
        }

        private void SaveDataSources(BaseParams baseParams, StoredReport report)
        {
            var relationDomain = this.Container.Resolve<IDomainService<StoredReportDataSource>>();
            var dataSourceDomain = this.Container.Resolve<IDomainService<DataSource>>();

            using (this.Container.Using(relationDomain, dataSourceDomain))
            {
                var existRelations = relationDomain.GetAll()
                .Where(x => x.StoredReport != null)
                .Where(x => x.StoredReport.Id == report.Id)
                .ToList();
                var dataSourcesIds = baseParams.Params.GetAs<string>("DataSourcesIds", ignoreCase: true).ToLongArray();

                var relationsToRemove = existRelations.Where(x => !dataSourcesIds.Contains(x.DataSource.Id)).Select(x => x.Id);
                relationsToRemove.ForEach(x => relationDomain.Delete(x));

                if (dataSourcesIds.Length > 0)
                {
                    var dataSourcesIdsForNewRelations = dataSourcesIds.Where(x => !existRelations.Select(y => y.DataSource.Id).Contains(x)).ToList();
                    var dataSources = dataSourceDomain.GetAll().Where(x => dataSourcesIdsForNewRelations.Contains(x.Id)).ToList();

                    foreach (var dataSource in dataSources)
                    {
                        relationDomain.Save(new StoredReportDataSource
                        {
                            ObjectCreateDate = DateTime.Now,
                            ObjectEditDate = DateTime.Now,
                            ObjectVersion = 0,
                            DataSource = dataSource,
                            StoredReport = report
                        });
                    }
                }
            }
        }

        private void SaveReportParams(BaseParams baseParams, StoredReport report)
        {
            var reportParamDomain = this.Container.Resolve<IDomainService<ReportParamGkh>>();
            var reportParams = baseParams.Params.GetAs("reportParams", ignoreCase: true, defaultValue: new ReportParamGkh[0]);
            var paramsIdsToDelete = baseParams.Params.GetAs<string>("DeletedParamsIds", ignoreCase: true).ToLongArray();

            using (this.Container.Using(reportParamDomain))
            {
                foreach (var id in paramsIdsToDelete)
                {
                    if (id > 0)
                    {
                        if (reportParamDomain.GetAll().Any(x => x.Id == id))
                        {
                            reportParamDomain.Delete(id);
                        }
                    }
                }

                foreach (var reportParam in reportParams)
                {
                    report.AddParam(reportParam);
                    if (reportParam.Id > 0)
                    {
                        reportParamDomain.Update(reportParam);
                    }
                    else
                    {
                        reportParamDomain.Save(reportParam);
                    }
                }
            }
        }
    }
}
