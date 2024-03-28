namespace Bars.GkhCr.Report
{
	using B4.Modules.Reports;
	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Enums;
	using Bars.GkhCr.Entities;
	using Bars.GkhCr.Localizers;
	using Castle.Windsor;
	using System;
	using System.Collections.Generic;
	using System.Linq;

    /// <summary>
    /// Отчет "Сведения по актам и протоколам объекта КР".
    /// </summary>
    public class InfoActProtocolObjectCr : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime reportDate = DateTime.Now;
        private int programCrId;
        private List<long> municipalityIds = new List<long>();

        public InfoActProtocolObjectCr()
            : base(new ReportTemplateBinary(Properties.Resources.InfoActProtocolObjectCr))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.InfoActProtocolObjectCr";
            }
        }

        public override string Desciption
        {
            get { return "Отчет \"Сведения по актам и протоколам объекта КР\""; }
        }

        public override string GroupName
        {
            get { return "Отчеты Кап.ремонт"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.InfoActProtocolObjectCr"; }
        }

        public override string Name
        {
            get { return "Отчет \"Сведения по актам и протоколам объекта КР\""; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.reportDate = baseParams.Params["reportDate"].ToDateTime();
            this.programCrId = baseParams.Params["programCrId"].ToInt();

            this.municipalityIds.Clear();

            var municipalityStr = baseParams.Params["municipalityIds"].ToString();
            if (!string.IsNullOrEmpty(municipalityStr))
            {
                var mcp = municipalityStr.Split(',');
                foreach (var id in mcp)
                {
                    var mcp_id = 0;
                    if (int.TryParse(id, out mcp_id))
                    {
                        if (!this.municipalityIds.Contains(mcp_id))
                        {
                            this.municipalityIds.Add(mcp_id);
                        }
                    }
                }
            }
        }

        public override string ReportGenerator { get; set; }

        /// <summary>
        /// The prepare report.
        /// </summary>
        /// <param name="reportParams">
        /// The report parameters.
        /// </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var workcodes = new List<string> { "7", "8", "9", "10", "11" };

            var municipalityDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                    .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Id))
                    .Select(x => new { x.Id, x.Name })
                    .AsEnumerable()
                    .OrderBy(x => x.Name)
                    .ToDictionary(x => x.Id, x => x.Name);

            var objectsCrQuery = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id));

            var objectCrIds = objectsCrQuery.Select(x => x.Id);
            var realtyObjIds = objectsCrQuery.Select(x => x.RealityObject.Id);

            var realtyObjByMunicipality = objectsCrQuery
                .Select(x => new
                {
                    municipalityId = x.RealityObject.Municipality.Id,
                    realtyObjId = x.RealityObject.Id,
                })
                .AsEnumerable()
                .GroupBy(x => x.municipalityId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.realtyObjId).Distinct().ToList());

            var worksDict = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.Work.TypeWork == TypeWork.Work)
                .Where(x => objectCrIds.Contains(x.ObjectCr.Id))
                .Select(x => new
                {
                    MunicipalityId = x.ObjectCr.RealityObject.Municipality.Id,
                    RealtyObjectId = x.ObjectCr.RealityObject.Id,
                    WorkId = x.Work.Id,
                    WorkName = x.Work.Name,
                    WorkCode = x.Work.Code
                })
                .AsEnumerable()
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var dict = x.GroupBy(y => y.RealtyObjectId)
                                    .ToDictionary(
                                        y => y.Key,
                                        y => new
                                        {
                                            OnlyInstall = y.All(z => workcodes.Contains(z.WorkCode)),
                                            HasInstall = y.Any(z => workcodes.Contains(z.WorkCode)),
                                            NotOnlyInstall = y.Any(z => !workcodes.Contains(z.WorkCode)),
                                            WorkCount = y.Count()
                                        });
                        return new
                        {
                            NotComplexHouses = dict.Count(y => y.Value.OnlyInstall),
                            ComplexHouses = dict.Count(y => y.Value.NotOnlyInstall),
                            TotalWork = dict.Sum(y => y.Value.WorkCount),
                            NotComplexWorkCount = dict.Where(y => y.Value.OnlyInstall).Sum(y => y.Value.WorkCount),
                            ComplexWorkCount = dict.Where(y => y.Value.NotOnlyInstall).Sum(y => y.Value.WorkCount)
                        };
                    });

            var defects = this.Container.Resolve<IDomainService<DefectList>>().GetAll()
                .Where(x => objectCrIds.Contains(x.ObjectCr.Id))
                .Where(x => x.Work.TypeWork == TypeWork.Work)
                .Where(x => x.State.TypeId.Contains("defect") && x.State.Name.Contains("Согласовано ГЖИ"))
                .Select(x => x.ObjectCr.RealityObject.Municipality.Id)
                .AsEnumerable()
                .GroupBy(x => x)
                .ToDictionary(x => x.Key, x => x.Count());

            var protocolDict = this.Container.Resolve<IDomainService<ProtocolCr>>().GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .Where(x => objectCrIds.Contains(x.ObjectCr.Id))
                .Where(x => x.File != null)
                .GroupBy(x => new { muId = x.ObjectCr.RealityObject.Municipality.Id, typeDocCrCode = x.TypeDocumentCr.Glossary.Code })
                .Select(x => new
                {
                    x.Key,
                    count = x.Count()
                })
                .AsEnumerable()
                .GroupBy(x => x.Key.muId)
                .Select(z =>
                {
                    var typeDocDict = z.ToDictionary(y => y.Key.typeDocCrCode, y => y.count);

                    var needCrCount = typeDocDict.ContainsKey(TypeDocumentCrLocalizer.ProtocolNeedCrKey) ? typeDocDict[TypeDocumentCrLocalizer.ProtocolNeedCrKey] : 0;
                    var changeCrCount = typeDocDict.ContainsKey(TypeDocumentCrLocalizer.ProtocolChangeCrKey) ? typeDocDict[TypeDocumentCrLocalizer.ProtocolChangeCrKey] : 0;
                    var completeCrCount = typeDocDict.ContainsKey(TypeDocumentCrLocalizer.ProtocolCompleteCrKey) ? typeDocDict[TypeDocumentCrLocalizer.ProtocolCompleteCrKey] : 0;
                    var actExpluatationAfterCr = typeDocDict.ContainsKey(TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey) ? typeDocDict[TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey] : 0;
                    var municipality = z.Key;

                    return new { NeedCrCount = needCrCount, ChangeCrCount = changeCrCount, CompleteCrCount = completeCrCount, ActExpluatationAfterCR = actExpluatationAfterCr, Municipality = municipality };
                })
                .ToDictionary(x => x.Municipality);

            var photosDict = this.Container.Resolve<IDomainService<RealityObjectImage>>().GetAll()
                         .Where(x => realtyObjIds.Contains(x.RealityObject.Id))
                         .Select(x => new
                         {
                             realityId = x.RealityObject.Id,
                             x.ImagesGroup,
                             moId = x.RealityObject.Municipality.Id
                         })
                         .AsEnumerable()
                         .GroupBy(x => x.moId)
                         .Select(x => new
                         {
                             Before = x.Count(y => y.ImagesGroup == ImagesGroup.BeforeOverhaul),
                             InOverhaul = x.Count(y => y.ImagesGroup == ImagesGroup.InOverhaul),
                             After = x.Count(y => y.ImagesGroup == ImagesGroup.AfterOverhaul),
                             PictureHouse = x.Count(y => y.ImagesGroup == ImagesGroup.PictureHouse),
                             municipality = x.Key
                         })
                         .ToDictionary(x => x.municipality);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            reportParams.SimpleReportParams["ДатаОтчета"] = this.reportDate.ToShortDateString();
            
            var counter = 0;
            foreach (var municipality in realtyObjByMunicipality)
            {
                section.ДобавитьСтроку();

                section["Номер1"] = ++counter;
                if (municipalityDict.ContainsKey(municipality.Key))
                {
                    section["МуниципальноеОбразование"] = municipalityDict[municipality.Key];
                }

                section["КолДомовВсего"] = municipality.Value.Count;

                if (worksDict.ContainsKey(municipality.Key))
                {
                    var works = worksDict[municipality.Key];

                    section["КолДомовКомплРемонт"] = works.ComplexHouses;
                    section["КолДомовПУ"] = works.NotComplexHouses;
                    section["КолРаботВсего"] = works.TotalWork;
                    section["КолРаботКомплРемонт"] = works.ComplexWorkCount;
                    section["КолРаботПУ"] = works.NotComplexWorkCount;
                }
                else
                {
                    section["КолДомовКомплРемонт"] = 0;
                    section["КолДомовПУ"] = 0;
                    section["КолРаботВсего"] = 0;
                    section["КолРаботКомплРемонт"] = 0;
                    section["КолРаботПУ"] = 0;
                }

                if (photosDict.ContainsKey(municipality.Key))
                {
                    var photos = photosDict[municipality.Key];

                    section["ФотоматериалыДо"] = photos.Before;
                    section["ФотоматериалыВходе"] = photos.InOverhaul;
                    section["ФотоматериалыПосле"] = photos.After;
                    section["ФотоматериалыВЦелом"] = photos.PictureHouse;
                }
                else
                {
                    section["ФотоматериалыДо"] = 0;
                    section["ФотоматериалыВходе"] = 0;
                    section["ФотоматериалыПосле"] = 0;
                    section["ФотоматериалыВЦелом"] = 0;
                }

                if (defects.ContainsKey(municipality.Key))
                {
                    section["КолвоДефектов"] = defects[municipality.Key];
                }
                else
                {
                    section["КолвоДефектов"] = 0;
                }

                if (protocolDict.ContainsKey(municipality.Key))
                {
                    var protocol = protocolDict[municipality.Key];

                    section["КолПротоколовОНеобходимостиКапРемонта"] = protocol.NeedCrCount;
                    section["КолПротоколовОВнесенииИзменений"] = protocol.ChangeCrCount;
                    section["КолПротоколовОЗавершении"] = protocol.CompleteCrCount;
                    section["КолАктовВвода"] = protocol.ActExpluatationAfterCR;
                }
                else
                {
                    section["КолПротоколовОНеобходимостиКапРемонта"] = 0;
                    section["КолПротоколовОВнесенииИзменений"] = 0;
                    section["КолПротоколовОЗавершении"] = 0;
                    section["КолАктовВвода"] = 0;
                }
            }
            
            reportParams.SimpleReportParams["КолДомовВсегоСумма"] = realtyObjByMunicipality.Sum(x => x.Value.Count);

            reportParams.SimpleReportParams["КолДомовКомплСумма"] = worksDict.Sum(x => x.Value.ComplexHouses);
            reportParams.SimpleReportParams["КолДомовПУСумма"] = worksDict.Sum(x => x.Value.NotComplexHouses);
            reportParams.SimpleReportParams["КолРаботВсегоСумма"] = worksDict.Sum(x => x.Value.TotalWork);
            reportParams.SimpleReportParams["КолРаботКомплРемонтСумма"] = worksDict.Sum(x => x.Value.ComplexWorkCount);
            reportParams.SimpleReportParams["КолРаботПУСумма"] = worksDict.Sum(x => x.Value.NotComplexWorkCount);

            reportParams.SimpleReportParams["ФотоматериалыДоСумма"] = photosDict.Sum(x => x.Value.Before);
            reportParams.SimpleReportParams["ФотоматериалыВходеСумма"] = photosDict.Sum(x => x.Value.InOverhaul);
            reportParams.SimpleReportParams["ФотоматериалыПослеСумма"] = photosDict.Sum(x => x.Value.After);
            reportParams.SimpleReportParams["ФотоматериалыВЦеломСумма"] = photosDict.Sum(x => x.Value.PictureHouse);

            reportParams.SimpleReportParams["КолвоДефектов"] = defects.Sum(x => x.Value);

            reportParams.SimpleReportParams["КолПротоколовОНеобходимостиКапРемонтаСумма"] = protocolDict.Sum(x => x.Value.NeedCrCount);
            reportParams.SimpleReportParams["КолПротоколовОВнесенииИзмененийСумма"] = protocolDict.Sum(x => x.Value.ChangeCrCount);
            reportParams.SimpleReportParams["КолПротоколовОЗавершенииСумма"] = protocolDict.Sum(x => x.Value.CompleteCrCount);
            reportParams.SimpleReportParams["КолАктовВводаСумма"] = protocolDict.Sum(x => x.Value.ActExpluatationAfterCR);
        }
    }
}