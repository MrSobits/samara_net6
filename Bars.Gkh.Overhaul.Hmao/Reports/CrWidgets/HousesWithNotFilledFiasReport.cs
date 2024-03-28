namespace Bars.Gkh.Overhaul.Hmao.Reports.CrWidgets
{
    using System.Collections.Generic;
    using System.Linq;
    
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Properties;

    using Castle.Windsor;

    using IDpkrService = Bars.Gkh.Overhaul.Hmao.DomainService.IDpkrService;

    /// <summary>
    /// Выгрузка данных виджета "Дома, у которых в Реестре жилых домов не заполнен код ФИАС"
    /// </summary>
    public class HousesWithNotFilledFiasReport : DataExportReport
    {
        public IWindsorContainer Container { get; set; }

        private Dictionary<long, IEnumerable<ReportDataDto>> ReportData { get; set; }
        
        /// <inheritdoc />
        public HousesWithNotFilledFiasReport() : base(new ReportTemplateBinary(Resources.HousesWithNotFilledFias))
        {
        }

        /// <inheritdoc />
        public override string Name => "Статистические данные по КР. Дома, у которых в Реестре жилых домов не заполнен код ФИАС";

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
            var realityObjectStructuralElementDomain = this.Container.ResolveDomain<RealityObjectStructuralElement>();
            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();
            var dpkrService = this.Container.Resolve<IDpkrService>();

            using (this.Container.Using(realityObjectDomain, realityObjectStructuralElementDomain, dpkrService))
            {
                var baseQuery = dpkrService.GetRealityObjectBaseQuery(municipalityId);
                
                this.ReportData = baseQuery
                    .Where(x => !x.FiasAddress.HouseGuid.HasValue)
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key,
                        x => x.OrderBy(y => y.Address)
                            .Select(y => new ReportDataDto
                            {
                                MunicipalityName = y.Municipality.Name,
                                Address = y.Address
                            }));
            }
        }

        /// <summary>
        /// DTO данных для выгрузки 
        /// </summary>
        private class ReportDataDto
        {
            public string MunicipalityName { get; set; }
            public string Address { get; set; }
        }
    }
}