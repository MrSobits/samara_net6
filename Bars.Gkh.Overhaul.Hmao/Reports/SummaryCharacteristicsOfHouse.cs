namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Properties;

    using Castle.Windsor;

    class SummaryCharacteristicsOfHouse : BasePrintForm
    {
        public SummaryCharacteristicsOfHouse()
            : base(new ReportTemplateBinary(Resources.SummaryCharacteristicsOfHouse))
        {
        }

        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Сводная характеристика домов на ДПКР"; }
        }

        public override string Desciption
        {
            get { return "Сводная характеристика домов на ДПКР"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.SummaryCharacteristicsOfHouse";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GKH.SummaryCharacteristicsOfHouse";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var longTermPrObjectAll = this.Container.Resolve<IDomainService<VersionRecord>>().GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ProgramVersion.IsMain)
                .Select(x => new
                {
                    realObjId = x.RealityObject.Id,
                    MuId = x.RealityObject.Municipality.Id,
                    MuName = x.RealityObject.Municipality.Name,
                    LivingNotLivingArea = x.RealityObject.AreaLivingNotLivingMkd,
                    x.RealityObject.NumberApartments
                })
                .OrderBy(x => x.MuName)
                .ToList();
            
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var num = 1;

            var municipalityList = longTermPrObjectAll.Select(x => x.MuId).Distinct();
            var longTermPrObject = longTermPrObjectAll.Distinct(x => x.realObjId).ToList();

            foreach (var municipality in municipalityList)
            {
                section.ДобавитьСтроку();
                section["num"] = num++;
                foreach (var realObj in longTermPrObject)
                {
                    var realObjMuId = realObj.MuId;
                    var realObjByMu = longTermPrObject.Where(x => x.MuId == realObjMuId);

                    if (realObjMuId == municipality)
                    {
                        section["MU"] = realObj.MuName;
                        section["HouseCount"] = realObjByMu.Select(x => x.realObjId).Count();
                        section["TotalLiveNotLiveArea"] = realObjByMu.Select(x => x.LivingNotLivingArea).Sum();
                        section["AppartmentCount"] = realObjByMu.Select(x => x.NumberApartments).Sum();
                    }

                }
            }

          }
    }
}
