namespace Bars.Gkh.Overhaul.Hmao.Reports.CrWidgets
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Properties;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    
    /// <summary>
    /// Отчёт для виджета "Бюджетирование"
    /// </summary>
    public class CrBudgetingReport : DataExportReport
    {
        public IWindsorContainer Container { get; set; } 

        /// <inheritdoc />
        public CrBudgetingReport() : base(new ReportTemplateBinary(Resources.CrBudgetingReport))
        {
        }

        /// <inheritdoc />
        public override string Name => "Статистические данные по КР. Бюджетирование";
        
        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            var municipalityId = this.BaseParams.Params.GetAs<long>("municipalityId");
            var subsidyRecordVersionDomain = this.Container.ResolveDomain<SubsidyRecordVersion>();
            var dpkrService = this.Container.Resolve<IDpkrService>();

            Dictionary<string, IEnumerable<(string, decimal)>> data;

            using (this.Container.Using(subsidyRecordVersionDomain, dpkrService))
            {
                var municipalityIds = dpkrService.GetMunicipalityIds(); 
                
                var baseQuery = subsidyRecordVersionDomain.GetAll()
                    .Where(x => x.Version.IsMain)
                    .Where(x => x.CorrectionFinance != default(long))
                    .WhereIfElse(municipalityId != default(long), x => x.Version.Municipality.Id == municipalityId,
                        x => municipalityIds.Contains(x.Version.Municipality.Id));

                object reportData = null;

                if (municipalityId != default(long))
                {
                    reportData = baseQuery
                        .Select(x => new
                        {
                            Name = x.SubsidyYear,
                            Value = x.CorrectionFinance
                        })
                        .OrderBy(x => x.Name)
                        .AsEnumerable()
                        .Select(x => (x.Name.ToString(), x.Value)).ToList();
                    
                    var municipalityName = string.Empty;
                    this.Container.UsingForResolved<IDomainService<Municipality>>((_, service) =>
                    {
                        municipalityName = service.Load(municipalityId).Name;
                    });

                    data = new Dictionary<string, IEnumerable<(string, decimal)>>
                    {
                        { municipalityName, (IList<(string, decimal)>)reportData }
                    };
                }
                else
                {
                    reportData = baseQuery
                        .GroupBy(x => new { x.Version.Municipality.Id, x.Version.Municipality.Name, x.SubsidyYear },
                            (x, y) => new
                            {
                                x.Name,
                                x.SubsidyYear,
                                Value = y.Sum(z => z.CorrectionFinance)
                            })
                        .OrderBy(x => x.Name)
                        .ThenBy(x => x.SubsidyYear)
                        .AsEnumerable()
                        .Select(x => new CrBudgetingExportDto
                        {
                            Name = x.Name,
                            AdditionalData = x.SubsidyYear.ToString(),
                            Value = x.Value
                        })
                        .ToList();
                    
                    data = ((IList<CrBudgetingExportDto>)reportData)
                        .GroupBy(x => x.Name)
                        .ToDictionary(x => x.Key,
                            x => x.Select(y => (y.AdditionalData, y.Value)));
                }

                this.FillReportFile(data, reportParams);
            }
        }

        /// <summary>
        /// Метод заполенения файла отчёта
        /// </summary>
        /// <param name="data">данные</param>
        /// <param name="reportParams">параметры</param>
        private void FillReportFile(IDictionary<string, IEnumerable<(string, decimal)>> data, ReportParams reportParams)
        {
            var rowIndex = 0;
            var totalSum = 0m;
            
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
            
            foreach (var municipalityData in data)
            {
                var sumByMo = 0m;
                var municipalityName = municipalityData.Key;
                var years = municipalityData.Value;
                    
                section.ДобавитьСтроку();
                var sectionMo = section.ДобавитьСекцию("SectionMo");

                foreach (var yearData in years)
                {
                    sectionMo.ДобавитьСтроку();
                        
                    sectionMo["Num"] = ++rowIndex;
                    sectionMo["Municipality"] = municipalityName;
                    sectionMo["Year"] = yearData.Item1;
                    sectionMo["Value"] = yearData.Item2;

                    sumByMo += yearData.Item2;
                }

                section["TotalSumByMo"] = sumByMo;
                totalSum += sumByMo;
            }
            
            reportParams.SimpleReportParams["TotalSum"] = totalSum;
        }
    }
    
    /// <summary>
    /// DTO данных для выгрузки
    /// </summary>
    public class CrBudgetingExportDto
    {
        public decimal Value { get; set; }
        
        public string Name { get; set; }

        public string AdditionalData { get; set; }
    }
}