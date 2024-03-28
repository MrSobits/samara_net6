namespace Bars.Gkh.Overhaul.Hmao.Reports.CrWidgets
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Properties;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using NHibernate.Transform;

    /// <summary>
    /// Отчёт для виджета "Дома, попавшие в версии ДПКР"
    /// </summary>
    public class NotIncludedInCrHousesReport : DataExportReport
    {
        public IWindsorContainer Container { get; set; }

        private Dictionary<long, IEnumerable<ReportDataDto>> ReportData { get; set; }

        /// <inheritdoc />
        public NotIncludedInCrHousesReport() : base(new ReportTemplateBinary(Resources.NotIncludedInCrHouses))
        {
        }

        /// <inheritdoc />
        public override string Name => "Статистические данные по КР. Дома, не попавшие в версии ДПКР";
        
        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            this.PrepareData();
            
            var rowIndex = 0;
            
            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
            
            foreach (var muData in this.ReportData.OrderBy(x => x.Key))
            {
                sectionMu.ДобавитьСтроку();
                    
                foreach (var record in muData.Value)
                {
                    sectionRo.ДобавитьСтроку();
                    
                    sectionRo["seqNumber"] = ++rowIndex;
                    sectionRo["municipalityName"] = record.MunicipalityName;
                    sectionRo["address"] = record.Address;
                }

                sectionMu["countRoMu"] = muData.Value.Count();
            }

            reportParams.SimpleReportParams["countRoTotal"] = rowIndex;
        }

        private void PrepareData()
        {
            var municipalityId = this.BaseParams.Params.GetAs<long>("municipalityId");
            var dpkrService = this.Container.Resolve<IDpkrService>();

            using (this.Container.Using(dpkrService))
            {
                var data = dpkrService.GetNotIncludedInCrHousesQuery(false, municipalityId);

                using (var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession())
                {
                    var queryResult = session.CreateSQLQuery(data.Query)
                        .SetParams(data.Params)
                        .SetResultTransformer(Transformers.AliasToBean<ReportDataDto>())
                        .List<ReportDataDto>();

                    this.ReportData = queryResult
                        .GroupBy(x => x.MunicipalityId)
                        .ToDictionary(x => x.Key,
                            x => x.OrderBy(y => y.Address)
                                .Select(y => new ReportDataDto
                                {
                                    MunicipalityName = y.MunicipalityName,
                                    Address = y.Address,
                                }));
                }
            }
        }
        
        /// <summary>
        /// DTO данных для выгрузки 
        /// </summary>
        public class ReportDataDto
        {
            public long MunicipalityId { get; set; }
            public string MunicipalityName { get; set; }
            public string Address { get; set; }
        }
        
    }
}