namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Overhaul.Entities;
    using Properties;

    using Castle.Windsor;
    using Entities;
    using Gkh.Utils;

    class MarginalCostKr1LivingSpace : BasePrintForm
    {
        public MarginalCostKr1LivingSpace()
            : base(new ReportTemplateBinary(Resources.MarginalCostKr1LivingSpace))
        {
        }
        
        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }

        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionDomain  { get; set; }

        public IDomainService<VersionRecord> VersionRecordDomain  { get; set; }

        public override string Name
        {
            get { return "Предельная стоимость проведения комплексного КР на 1 кв.м. общей площади помещений"; }
        }

        public override string Desciption
        {
            get { return "Предельная стоимость проведения комплексного КР на 1 кв.м. общей площади помещений"; }
        }

        public override string GroupName
        {
            get { return "Региональная программа"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.MarginalCostKr1LivingSpace";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.MarginalCostKr1LivingSpace";
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
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;
            var rateCalcArea = config.RateCalcTypeArea;

            IEnumerable<DpkrCorrectionProxy> newData;

            if (groupByRoPeriod == 0)
            {
                newData = DpkrCorrectionDomain.GetAll()
                         .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                    .WhereIf(municipalityIds.Any(),
                        x => municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                         .Select(x => new DpkrCorrectionProxy
                         {
                             RealObjId = x.RealityObject.Id,
                             MunId = x.RealityObject.Municipality.Id,
                             MunName = x.RealityObject.Municipality.Name,
                             Address = x.RealityObject.Address,
                             Year = x.Stage2.Stage3Version.Year,
                             Sum = x.Stage2.Sum,
                             CommonEstateObjects = x.Stage2.CommonEstateObject.Name,
                             IndexNumber = x.Stage2.Stage3Version.IndexNumber,
                             AreaLiving = x.RealityObject.AreaLivingNotLivingMkd.HasValue 
                                    ? x.RealityObject.AreaLivingNotLivingMkd.Value 
                            : x.RealityObject.AreaLiving.HasValue
                                ? x.RealityObject.AreaLiving.Value
                                : 0
                    })
				.AsEnumerable();
            }
            else
            {
                var dataCorrection =
                    DpkrCorrectionDomain.GetAll()
                                 .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                        .WhereIf(municipalityIds.Any(),
                            x => municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                        .Select(x => new {x.Stage2.Stage3Version.Id, x.PlanYear})
                                 .AsEnumerable()
                                 .GroupBy(x => x.Id)
                                 .ToDictionary(x => x.Key, y => y.Select(x => x.PlanYear).FirstOrDefault());

                var query =
                    VersionRecordDomain.GetAll()
                                 .Where(x => x.ProgramVersion.IsMain)
                                 .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.ProgramVersion.Municipality.Id))
                                 .Where(x => DpkrCorrectionDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                        .Select(x => new
                                     {
                                         x.Id,
                                         RealObjId = x.RealityObject.Id,
                                         MunId = x.RealityObject.Municipality.Id,
                                         Municipality = x.RealityObject.Municipality.Name,
                                         RealityObject = x.RealityObject.Address,
                                         CorrectionYear = 0,
                                         PlanYear = x.Year,
                                         x.Sum,
                                         x.CommonEstateObjects,
                                         x.IndexNumber,
                            x.RealityObject.AreaLivingNotLivingMkd,
                            x.RealityObject.AreaLiving,
                            x.RealityObject.AreaMkd
                                     })
                                 .AsEnumerable();

                newData =
                    query
                        .Select(x => new DpkrCorrectionProxy
                        {
                             RealObjId = x.RealObjId,
                             MunId = x.MunId,
                             MunName = x.Municipality,
                             Address = x.RealityObject,
                             Year = dataCorrection.ContainsKey(x.Id) ? dataCorrection[x.Id] : 0,
                             Sum = x.Sum,
                             CommonEstateObjects = x.CommonEstateObjects,
                             IndexNumber = x.IndexNumber,
                            AreaLiving = x.AreaLiving.ToDecimal(),
                            AreaLivingNotLivingMkd = x.AreaLivingNotLivingMkd.ToDecimal(),
                            AreaMkd = x.AreaMkd.ToDecimal(),
                        });

            }


            //var dataRecord = this.Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>().GetAll()
            //    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
            //    .Select(x => new
            //    {
            //        realObjId = x.RealityObject.Id,
            //        MunId = x.RealityObject.Municipality.Id,
            //        x.IndexNumber,
            //        MunName = x.RealityObject.Municipality.Name,
            //        x.RealityObject.Address,
            //        x.CommonEstateObjects,
            //        x.Year,
            //        x.Sum,
            //        AreaLiving = x.RealityObject.AreaLivingNotLivingMkd.HasValue 
            //        ? x.RealityObject.AreaLivingNotLivingMkd.Value 
            //        : x.RealityObject.AreaLiving.HasValue ? x.RealityObject.AreaLiving.Value : 0
            //    })
            //    .OrderBy(x => x.IndexNumber)
            //    .ThenBy(x => x.MunName)
            //    .ToList();

            var dataRecord = newData.OrderBy(x => x.IndexNumber).ThenBy(x => x.MunName).Distinct().ToList();

            var dataRecId = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage3>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, 
                    x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                        || municipalityIds.Contains(x.RealityObject.MoSettlement.Id))
                .Select(x => x.RealityObject.Id);

            var workPrice = Container.Resolve<IDomainService<WorkPrice>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, 
                    x => municipalityIds.Contains(x.Municipality.Id))
                .Where(x => x.Job != null)
                .Where(x => x.Job.Name == "Комплексный ремонт для домов в деревянном исполнении" 
                    || x.Job.Name == "Комплексный ремонт для домов в капитальном исполнении")
                .Where(x => x.Year == 2014)
                .Select(x => new
                {
                    MunId = x.Municipality.Id,
                    job = x.Job.Name == "Комплексный ремонт для домов в деревянном исполнении" ? "ДИ" : "КИ",
                    x.SquareMeterCost
                })
                .AsEnumerable()
                .GroupBy(x => x.MunId)
                .ToDictionary(
                x => x.Key,
                x => x.GroupBy(y => y.job)
                    .ToDictionary(y => y.Key, y => y.Select(z => z.SquareMeterCost).FirstOrDefault()));
            
            var constElInDpkr = Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .Where(x => dataRecId.Contains(x.RealityObject.Id))
                .Where(x => x.StructuralElement.Name == "Деревянный")
                .Where(x => x.StructuralElement.Group.Name == "Тип фасада")
                .Where(x => x.StructuralElement.Group.CommonEstateObject.Name == "Фасад")
                .Where(x => x.State.StartState)
                .Select(x => x.RealityObject.Id)
                .ToList();

            var num = 1;
            var rowCount = 0;
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var realObj in dataRecord)
            {
                decimal? workPriceWood = null;
                decimal? workPriceCapital = null;

                if (workPrice.ContainsKey(realObj.MunId))
                {
                    if (workPrice[realObj.MunId].ContainsKey("ДИ"))
                    {
                        workPriceWood = workPrice[realObj.MunId]["ДИ"];
                    }

                    if (workPrice[realObj.MunId].ContainsKey("КИ"))
                    {
                        workPriceCapital = workPrice[realObj.MunId]["КИ"];
                    }
                }

                section.ДобавитьСтроку();
                rowCount++;
                if (rowCount >= 65000)
                {
                    throw new Exception("Количество строк превышает 65000, выберите МО для которой расчитан ДПКР");
                }

                var area = rateCalcArea == RateCalcTypeArea.AreaLiving ? realObj.AreaLiving :
                   rateCalcArea == RateCalcTypeArea.AreaMkd ? realObj.AreaMkd :
                   rateCalcArea == RateCalcTypeArea.AreaLivingNotLiving ? realObj.AreaLivingNotLivingMkd : 0M;

                var cost = area > 0 ? realObj.Sum.ToDecimal() / area : 0;
                section["Num"] = num++;
                section["NumInTurn"] = realObj.IndexNumber;
                section["Mun"] = realObj.MunName;
                section["Address"] = realObj.Address;
                section["OOI"] = realObj.CommonEstateObjects;
                section["Year"] = realObj.Year;
                section["Cost"] = realObj.Sum;

                if (constElInDpkr.Contains(realObj.RealObjId))
                {
                    section["CostTree"] = cost.RoundDecimal(2);

                    if (workPriceWood.HasValue)
                    {
                        section["LimitCostTree"] = workPriceWood.Value;
                        section["woodYellow"] = cost > workPriceWood.Value ? 1 : 0;
                    }
                }

                if (!constElInDpkr.Contains(realObj.RealObjId))
                {
                    section["CostCap"] = cost.RoundDecimal(2);

                    if (workPriceCapital.HasValue)
                    {
                        section["LimitCostCap"] = workPriceCapital.Value;
                        section["capitalYellow"] = cost > workPriceCapital.Value ? 1 : 0;
                    }
                }
            }
        }

        private class DpkrCorrectionProxy
        {
            public long RealObjId { get; set; }

            public long MunId { get; set; }

            public int IndexNumber { get; set; }

            public string MunName { get; set; }

            public string Address { get; set; }

            public string CommonEstateObjects { get; set; }

            public int Year { get; set; }

            public decimal Sum { get; set; }

            public decimal AreaLiving { get; set; }

            public decimal AreaLivingNotLivingMkd { get; set; }

            public decimal AreaMkd { get; set; }
        }
    }
}
