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

    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.IoC;

    public class CountPhotoReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long programCrId;
        private List<long> municipalityListId = new List<long>();

        /// <summary>
        /// коды работ, по которым собирается отчет
        /// Для этого отчета важен порядок и коды, по которым собирается отчет
        /// </summary>
        private readonly List<string> allWorkCodes = new List<string>
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
            "11", "12", "13", "14", "141", "142", "143", "15", "16", "17", "18", "19", "20",
            "21", "22", "23", "29", "88", "145"
        };

        public CountPhotoReport()
            : base(new ReportTemplateBinary(Properties.Resources.CountPhoto))
        {
        }

        public override string RequiredPermission => "Reports.CR.CountPhoto";

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption => "Отчет \"Количество фотографий\"";

        /// <summary>
        /// Группа
        /// </summary>
        public override string GroupName => "Отчеты Кап.ремонт";

        /// <summary>
        /// Представление с пользователскими параметрами
        /// </summary>
        public override string ParamsController => "B4.controller.report.CountPhoto";

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Отчет \"Количество фотографий\"";

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIds = baseParams.Params.GetAs<string>("municipalityIds");

            this.programCrId = baseParams.Params["programCrId"].ToInt();
            this.municipalityListId = !string.IsNullOrEmpty(municipalityIds) ? municipalityIds.Split(',').Select(id => id.ToLong()).ToList(): new List<long>();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var programCrDomain = this.Container.Resolve<IDomainService<ProgramCr>>();
            var objectCrDomain = this.Container.Resolve<IDomainService<ObjectCr>>();
            var buildContractDomain = this.Container.Resolve<IDomainService<BuildContract>>();
            var typeWorkCrDomain = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var protocolCrDomain = this.Container.Resolve<IDomainService<ProtocolCr>>();
            var crObjImageDomain = this.Container.Resolve<IDomainService<RealityObjectImage>>();
            var realityObjImageDomain = this.Container.Resolve<IDomainService<RealityObjectImage>>();

            using (this.Container.Using(programCrDomain,
                objectCrDomain,
                buildContractDomain,
                typeWorkCrDomain,
                protocolCrDomain,
                crObjImageDomain,
                realityObjImageDomain))
            {
                var programCr = programCrDomain.GetAll()
                    .FirstOrDefault(x => x.Id == this.programCrId);

                if (programCr == null)
                {
                    return;
                }

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

                var queryObjectCr = objectCrDomain.GetAll()
                    .Where(x => x.ProgramCr.Id == this.programCrId)
                    .WhereIf(this.municipalityListId.IsNotEmpty(), x => this.municipalityListId.Contains(x.RealityObject.Municipality.Id));

                var queryObjectCrId = queryObjectCr.Select(x => x.Id);

                var listBuildContract = buildContractDomain.GetAll()
                    .Where(x => queryObjectCrId.Contains(x.ObjectCr.Id))
                    .Select(x => new { ObjectCrId = x.ObjectCr.Id, BuilderName = x.Builder.Contragent.Name })
                    .ToList();

                var listObjectCr = queryObjectCr.Select(x => new
                    {
                        CrObjectId = x.Id,
                        RoId = x.RealityObject.Id,
                        x.GjiNum,
                        x.RealityObject.Address,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                    })
                    .ToList();

                var listTypeWorkCrObject = typeWorkCrDomain.GetAll()
                    .Where(x => queryObjectCr.Any(y => y.Id == x.ObjectCr.Id))
                    .Select(x => new
                    {
                        RoId = x.ObjectCr.RealityObject.Id,
                        WorkCode = x.Work.Code
                    });

                var queryObjectCrIds = queryObjectCr.Select(x => x.Id);

                var dictProtocolCrInfo = protocolCrDomain.GetAll()
                    .Where(x => queryObjectCrIds.Contains(x.ObjectCr.Id))
                    .Select(x => new { RoId = x.ObjectCr.RealityObject.Id, TypeDocument = x.TypeDocumentCr.Key })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(
                        x => x.Key,
                        v => v.Select(y => y.TypeDocument).ToList());

                var groupedBycrObject = listObjectCr.GroupBy(x => x.CrObjectId)
                    .Select(x =>
                    {
                        var listBuilders = listBuildContract.Where(y => y.ObjectCrId == x.Key)
                            .Select(y => y.BuilderName)
                            .Distinct()
                            .ToList();

                        var builders = string.Empty;

                        if (listBuilders.Count > 0)
                        {
                            builders = listBuilders.Aggregate((curr, next) => $"{curr};{next}");
                        }

                        return new
                        {
                            Id = x.Select(y => y.RoId).FirstOrDefault(),
                            MunicipalityName = x.Select(y => y.MunicipalityName).FirstOrDefault(),
                            GjiNum = x.Select(y => y.GjiNum).FirstOrDefault(),
                            Address = x.Select(y => y.Address).FirstOrDefault(),
                            RoId = x.Select(y => y.RoId).FirstOrDefault(),
                            Builders = builders
                        };
                    })
                    .OrderBy(x => x.MunicipalityName).ToList();

                var realityObjIdsQuery = queryObjectCr.Select(x => x.RealityObject.Id);

                var realityObjImages = crObjImageDomain.GetAll()
                    .Where(x => programCr.Period.Id == x.Period.Id && realityObjIdsQuery.Contains(x.RealityObject.Id) &&
                        this.allWorkCodes.Contains(x.WorkCr.Code))
                    .Select(x => new
                    {
                        RealityObjId = x.RealityObject.Id,
                        WorkCRCode = x.WorkCr.Code,
                        x.ImagesGroup
                    })
                    .ToList();

                var groupByCodeandImgGroup = realityObjImages
                    .Where(y => y.WorkCRCode != null)
                    .GroupBy(x => new { x.RealityObjId, x.ImagesGroup, x.WorkCRCode })
                    .Select(x => new
                    {
                        x.Key.RealityObjId,
                        x.Key.WorkCRCode,
                        x.Key.ImagesGroup,
                        Count = x.Count()
                    })
                    .ToList();

                // Количество фотографий с группой - "Изображение жилого дома"
                var pictureHouseDict = realityObjImageDomain.GetAll()
                    .Where(x => realityObjIdsQuery.Contains(x.RealityObject.Id))
                    .Where(x => x.ImagesGroup == ImagesGroup.PictureHouse)
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, x => x.Count());

                var homeWorkCodes = groupByCodeandImgGroup.GroupBy(x => x.RealityObjId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.WorkCRCode).Distinct().ToList());

                var imagegroups = new List<ImagesGroup>
                {
                    ImagesGroup.AfterOverhaul,
                    ImagesGroup.BeforeOverhaul,
                    ImagesGroup.InOverhaul
                };

                var groupByCodeandImgGroup2 = groupedBycrObject
                    .ToDictionary(
                        z => z.Id,
                        z =>
                            this.allWorkCodes.ToDictionary(
                                x => x,
                                x => imagegroups.ToDictionary(y => y, y => (int?) null)
                            )
                    );

                // если у дома есть один из видов работ то вместо прочерка 0
                //foreach (var realtyObj in homeWorkCodes)
                //{
                //    foreach (var wc in realtyObj.Value)
                //    {
                //        foreach (var img in imagegroups)
                //        {
                //            if (groupByCodeandImgGroup2.ContainsKey(realtyObj.Key)
                //                && groupByCodeandImgGroup2[realtyObj.Key].ContainsKey(wc)
                //                && groupByCodeandImgGroup2[realtyObj.Key][wc].ContainsKey(img))
                //            {
                //                groupByCodeandImgGroup2[realtyObj.Key][wc][img] = 0;
                //            }
                //        }
                //    }
                //}

                groupByCodeandImgGroup.ForEach(
                    x =>
                    {
                        if (groupByCodeandImgGroup2.ContainsKey(x.RealityObjId)
                            && groupByCodeandImgGroup2[x.RealityObjId].ContainsKey(x.WorkCRCode)
                            && groupByCodeandImgGroup2[x.RealityObjId][x.WorkCRCode].ContainsKey(x.ImagesGroup))
                        {
                            groupByCodeandImgGroup2[x.RealityObjId][x.WorkCRCode][x.ImagesGroup] = x.Count;
                        }
                    });

                var countTotalBeforeDict = this.allWorkCodes.ToDictionary(x => x, x => 0);
                var countTotalInDict = this.allWorkCodes.ToDictionary(x => x, x => 0);
                var countTotalAfterDict = this.allWorkCodes.ToDictionary(x => x, x => 0);
                var countTotalGenerallyDict = this.allWorkCodes.ToDictionary(x => x, x => 0);

                countTotalBeforeDict.Add("Total", 0);
                countTotalInDict.Add("Total", 0);
                countTotalAfterDict.Add("Total", 0);
                countTotalGenerallyDict.Add("Total", 0);

                var countBeforeDash = new Dictionary<string, int>();
                var countInDash = new Dictionary<string, int>();
                var countAfterDash = new Dictionary<string, int>();

                foreach (var crObject in groupedBycrObject)
                {
                    section.ДобавитьСтроку();

                    section["Municipality"] = crObject.MunicipalityName;
                    section["Builder"] = crObject.Builders;
                    section["GJINumber"] = crObject.GjiNum;
                    section["Address"] = crObject.Address;

                    int countTotalBefore = 0;
                    int countTotalIn = 0;
                    int countTotalAfter = 0;

                    var crObjImg = groupByCodeandImgGroup2.ContainsKey(crObject.Id)
                        ? groupByCodeandImgGroup2[crObject.Id]
                        : new Dictionary<string, Dictionary<ImagesGroup, int?>>();

                    var listTypeWorkCrObjectbyRoId = listTypeWorkCrObject.Where(x => x.RoId == crObject.Id);
                    foreach (var workCode in this.allWorkCodes)
                    {
                        var hasWorkOnObject = listTypeWorkCrObjectbyRoId.Any(x => x.WorkCode == workCode);

                        var images = crObjImg.ContainsKey(workCode) ? crObjImg[workCode] : new Dictionary<ImagesGroup, int?>();

                        int? beforeOverhaul = images.ContainsKey(ImagesGroup.BeforeOverhaul) ? images[ImagesGroup.BeforeOverhaul] : null;
                        int? inOverhaul = images.ContainsKey(ImagesGroup.InOverhaul) ? images[ImagesGroup.InOverhaul] : null;
                        int? afterOverhaul = images.ContainsKey(ImagesGroup.AfterOverhaul) ? images[ImagesGroup.AfterOverhaul] : null;


                        section[$"CountBefore_{workCode}"] = beforeOverhaul == null
                            ? hasWorkOnObject
                                ? "0"
                                : "-"
                            : beforeOverhaul.ToString();

                        countTotalBefore += beforeOverhaul ?? 0;
                        if (beforeOverhaul == null)
                        {
                            if (countBeforeDash.ContainsKey(workCode))
                            {
                                countBeforeDash[workCode] += 1;
                            }
                            else
                            {
                                countBeforeDash.Add(workCode, 1);
                            }
                        }

                        section[$"CountIn_{workCode}"] = inOverhaul == null
                            ? hasWorkOnObject
                                ? "0"
                                : "-"
                            : inOverhaul.ToString();

                        countTotalIn += inOverhaul ?? 0;
                        if (inOverhaul == null)
                        {
                            if (countInDash.ContainsKey(workCode))
                            {
                                countInDash[workCode] += 1;
                            }
                            else
                            {
                                countInDash.Add(workCode, 1);
                            }
                        }

                        section[$"CountAfter_{workCode}"] = afterOverhaul == null
                            ? hasWorkOnObject
                                ? "0"
                                : "-"
                            : afterOverhaul.ToString();

                        countTotalAfter += afterOverhaul ?? 0;
                        if (afterOverhaul == null)
                        {
                            if (countAfterDash.ContainsKey(workCode))
                            {
                                countAfterDash[workCode] += 1;
                            }
                            else
                            {
                                countAfterDash.Add(workCode, 1);
                            }
                        }

                        countTotalBeforeDict[workCode] += beforeOverhaul ?? 0;
                        countTotalInDict[workCode] += inOverhaul ?? 0;
                        countTotalAfterDict[workCode] += afterOverhaul ?? 0;
                    }

                    section["CountTotalBefore"] = countTotalBefore;
                    section["CountTotalIn"] = countTotalIn;
                    section["CountTotalAfter"] = countTotalAfter;
                    var countPictHouse = pictureHouseDict.ContainsKey(crObject.Id) ? pictureHouseDict[crObject.Id] : 0;
                    section["CountTotalGenerally"] = countPictHouse;

                    var protocolCrInfoCr = dictProtocolCrInfo.ContainsKey(crObject.Id) ? dictProtocolCrInfo[crObject.Id] : null;
                    section["CountGenerally"] = protocolCrInfoCr?.Count(x => x == TypeDocumentCrLocalizer.ProtocolNeedCrKey) ?? 0;
                    section["CountCompletion"] = protocolCrInfoCr?.Count(x => x == TypeDocumentCrLocalizer.ProtocolCompleteCrKey) ?? 0;
                    section["CountActCommissioning"] = protocolCrInfoCr?.Count(x => x == TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey) ?? 0;

                    countTotalBeforeDict["Total"] += countTotalBefore;
                    countTotalInDict["Total"] += countTotalIn;
                    countTotalAfterDict["Total"] += countTotalAfter;
                    countTotalGenerallyDict["Total"] += countPictHouse;
                }

                var countCrObj = groupByCodeandImgGroup2.Keys.Count;

                foreach (var workCode in this.allWorkCodes)
                {
                    reportParams.SimpleReportParams[$"TotalCountBefore_{workCode}"] =
                        (countBeforeDash.ContainsKey(workCode) && countBeforeDash[workCode] == countCrObj) ? "-" : countTotalBeforeDict[workCode].ToStr();
                    reportParams.SimpleReportParams[$"TotalCountIn_{workCode}"] =
                        (countInDash.ContainsKey(workCode) && countInDash[workCode] == countCrObj) ? "-" : countTotalInDict[workCode].ToStr();
                    reportParams.SimpleReportParams[$"TotalCountAfter_{workCode}"] =
                        (countAfterDash.ContainsKey(workCode) && countAfterDash[workCode] == countCrObj) ? "-" : countTotalAfterDict[workCode].ToStr();
                }

                reportParams.SimpleReportParams["TotalBefore"] = countTotalBeforeDict["Total"];
                reportParams.SimpleReportParams["TotalIn"] = countTotalInDict["Total"];
                reportParams.SimpleReportParams["TotalAfter"] = countTotalAfterDict["Total"];
                reportParams.SimpleReportParams["TotalGenerally"] = countTotalGenerallyDict["Total"];

                var valuesDictProtocolCrInfo = dictProtocolCrInfo.Values.SelectMany(x => x);
                reportParams.SimpleReportParams["TotalCountGenerally"] =
                    valuesDictProtocolCrInfo.Count(x => x == TypeDocumentCrLocalizer.ProtocolNeedCrKey);
                reportParams.SimpleReportParams["TotalCountCompletion"] =
                    valuesDictProtocolCrInfo.Count(x => x == TypeDocumentCrLocalizer.ProtocolCompleteCrKey);
                reportParams.SimpleReportParams["TotalCountActCommissioning"] =
                    valuesDictProtocolCrInfo.Count(x => x == TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey);
            }
        }

        public override string ReportGenerator
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}