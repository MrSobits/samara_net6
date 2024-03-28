namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhRf.Entities;

    using Castle.Windsor;

    public class CreatedRealtyObject : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long[] programCrIds;

        private long[] municipalityIds;

        private int assemblyTo = 10;

        public CreatedRealtyObject()
            : base(new ReportTemplateBinary(Properties.Resources.CreatedRealtyObject))
        {
        }

        public override string Name
        {
            get
            {
                return "Отчет о созданных объектах недвижимости";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет о созданных объектах недвижимости";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Ход капремонта";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.CreatedRealtyObject";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.CreatedRealtyObject";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var programCrIdsList = baseParams.Params.GetAs("programCrIds", string.Empty);
            programCrIds = !string.IsNullOrEmpty(programCrIdsList) ? programCrIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];                              

            // 10 - по всем домам; 20 - по наличию договора с ГИСУ
            assemblyTo = baseParams.Params["assemblyTo"].ToInt();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var servContractRfObject = Container.Resolve<IDomainService<ContractRfObject>>();
            var servObjectCr = Container.Resolve<IDomainService<ObjectCr>>();
            var servRealityObject = Container.Resolve<IDomainService<RealityObject>>();
            var servManOrgContractRealityObject = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var servRealityObjectMeteringDevice = Container.Resolve<IDomainService<RealityObjectMeteringDevice>>();

            var municipalityDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                       .WhereIf(this.municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                       .Select(x => new
                       {
                           muId = x.Id,
                           muName = x.Name
                       })
                       .ToDictionary(x => x.muId, x => x.muName);

            var allRealObjByGisuQuery = servContractRfObject.GetAll()
                         .WhereIf(this.municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                         .Select(x => x.RealityObject.Id);

            // столбцы: 3, 4, 5, 7, 8, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32
            var realtyObjectCrQuery = programCrIds.Any() ? servObjectCr.GetAll()
                .WhereIf(assemblyTo == 20, x => allRealObjByGisuQuery.Any(y => y == x.RealityObject.Id))
                .Where(x => programCrIds.Contains(x.ProgramCr.Id))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    realityId = x.RealityObject.Id,
                    dateCreated = x.ObjectCreateDate,
                    dateEdited = x.ObjectEditDate,
                    nameMo = x.RealityObject.Municipality.Name,
                    moId = x.RealityObject.Municipality.Id,
                    address = x.RealityObject.Address,
                    floors = x.RealityObject.MaximumFloors ?? 0,
                    series = x.RealityObject.SeriesHome,
                    groupCap = x.RealityObject.CapitalGroup.Name,
                    type = x.RealityObject.TypeHouse.GetEnumMeta().Display,
                    condition = x.RealityObject.ConditionHouse.GetEnumMeta().Display,
                    areaMkd = x.RealityObject.AreaMkd ?? 0M,
                    areaLivNotLivMkd = x.RealityObject.AreaLivingNotLivingMkd ?? 0M,
                    areaLiv = x.RealityObject.AreaLiving ?? 0M,
                    areaLivOwned = x.RealityObject.AreaLivingOwned ?? 0M,
                    numberEntrances = x.RealityObject.NumberEntrances ?? 0,
                    numberApartments = x.RealityObject.NumberApartments ?? 0,
                    numberLifts = x.RealityObject.NumberLifts ?? 0,
                    numberLiving = x.RealityObject.NumberLiving ?? 0,
                    basement = x.RealityObject.HavingBasement.GetEnumMeta().Display,
                    areaBasement = x.RealityObject.AreaBasement ?? 0M,
                    heatingSystem = x.RealityObject.HeatingSystem.GetEnumMeta().Display,
                    wallMaterial = x.RealityObject.WallMaterial.Name,
                    roofingMaterial = x.RealityObject.RoofingMaterial.Name,
                    typeRoof = x.RealityObject.TypeRoof.GetEnumMeta().Display,
                    dateCommissioning = x.RealityObject.DateCommissioning.HasValue ? x.RealityObject.DateCommissioning.Value.Year : DateTime.MaxValue.Year,
                    physicalWear = x.RealityObject.PhysicalWear ?? 0M,
                    dateLastOverhaul = x.RealityObject.DateLastOverhaul.HasValue ? x.RealityObject.DateLastOverhaul.Value.Year : DateTime.MaxValue.Year
                })
            : servRealityObject.GetAll()
                     .WhereIf(assemblyTo == 20, x => allRealObjByGisuQuery.Any(y => y == x.Id))
                     .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                     .Select(
                         x => new
                                  {
                                      realityId = x.Id,
                                      dateCreated = x.ObjectCreateDate,
                                      dateEdited = x.ObjectEditDate,
                                      nameMo = x.Municipality.Name,
                                      moId = x.Municipality.Id,
                                      address = x.Address,
                                      floors = x.MaximumFloors ?? 0,
                                      series = x.SeriesHome,
                                      groupCap = x.CapitalGroup.Name,
                                      type = x.TypeHouse.GetEnumMeta().Display,
                                      condition = x.ConditionHouse.GetEnumMeta().Display,
                                      areaMkd = x.AreaMkd ?? 0M,
                                      areaLivNotLivMkd = x.AreaLivingNotLivingMkd ?? 0M,
                                      areaLiv = x.AreaLiving ?? 0M,
                                      areaLivOwned = x.AreaLivingOwned ?? 0M,
                                      numberEntrances = x.NumberEntrances ?? 0,
                                      numberApartments = x.NumberApartments ?? 0,
                                      numberLifts = x.NumberLifts ?? 0,
                                      numberLiving = x.NumberLiving ?? 0,
                                      basement = x.HavingBasement.GetEnumMeta().Display,
                                      areaBasement = x.AreaBasement ?? 0M,
                                      heatingSystem = x.HeatingSystem.GetEnumMeta().Display,
                                      wallMaterial = x.WallMaterial.Name,
                                      roofingMaterial = x.RoofingMaterial.Name,
                                      typeRoof = x.TypeRoof.GetEnumMeta().Display,
                                      dateCommissioning =
                                  x.DateCommissioning.HasValue ? x.DateCommissioning.Value.Year : DateTime.MaxValue.Year,
                                      physicalWear = x.PhysicalWear ?? 0M,
                                      dateLastOverhaul =
                                  x.DateLastOverhaul.HasValue ? x.DateLastOverhaul.Value.Year : DateTime.MaxValue.Year
                                  });

            var realityIdQuery = realtyObjectCrQuery.Select(x => x.realityId).Distinct();

            // RealityObjId - NameProgramCrList
            var nameProgramCrDict = servObjectCr.GetAll()
                .Where(x => realityIdQuery.Contains(x.RealityObject.Id)
                     && x.ProgramCr.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden
                     && x.ProgramCr.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Print)
                     .Select(x => new
                                      {
                                          realityId = x.RealityObject.Id, 
                                          nameProgCr = x.ProgramCr.Name
                                      })
                     .AsEnumerable()
                     .GroupBy(x => x.realityId)
                     .ToDictionary(x => x.Key, x => x.Select(y => y.nameProgCr).ToList());

            var realtyObjectCrByMo = realtyObjectCrQuery
                                 .OrderBy(x => x.nameMo)
                                 .ThenBy(x => x.address)
                                 .AsEnumerable()
                                 .GroupBy(x => x.moId)
                                 .ToDictionary(x => x.Key, x => x.GroupBy(z => z.realityId).ToDictionary(y => y.Key, y => y.ToList()));

            // столбец 6
            var infoRealtyObjByGisu = servContractRfObject.GetAll()
                    .Where(x => realityIdQuery.Contains(x.RealityObject.Id))
                    .Select(x => new
                    {
                        moId = x.RealityObject.Municipality.Id,
                        realityId = x.RealityObject.Id,
                        numDoc = x.ContractRf.DocumentNum,
                        dateDoc = x.ContractRf.DocumentDate ?? DateTime.MinValue
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.moId)
                    .ToDictionary(x => x.Key, x => x.GroupBy(z => z.realityId).ToDictionary(y => y.Key, y => y.ToList()));

            // столбцы: 9, 10
            var infoManorgByReality = servManOrgContractRealityObject.GetAll()
                    .Where(x => realityIdQuery.Contains(x.RealityObject.Id))
                         .Select(x => new
                         {
                             moId = x.RealityObject.Municipality.Id,
                             realityId = x.RealityObject.Id,
                             typeManag = x.ManOrgContract.ManagingOrganization != null
                             ? x.ManOrgContract.ManagingOrganization.TypeManagement.GetEnumMeta().Display
                             : string.Empty,
                             nameManOrg = x.ManOrgContract.ManagingOrganization != null
                             ? x.ManOrgContract.ManagingOrganization.Contragent.Name
                             : string.Empty
                         })
                        .AsEnumerable()
                        .GroupBy(x => x.moId)
                        .ToDictionary(x => x.Key, x => x.GroupBy(z => z.realityId).ToDictionary(y => y.Key, y => y.ToList()));

            // столбец 33
            var infoDeviceByReality = servRealityObjectMeteringDevice.GetAll()
                    .Where(x => realityIdQuery.Contains(x.RealityObject.Id))
                         .Select(x => new
                         {
                             moId = x.RealityObject.Municipality.Id,
                             realityId = x.RealityObject.Id
                         })
                        .AsEnumerable()
                        .GroupBy(x => x.moId)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.realityId).ToList());

            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияМО");
            var section = sectionMo.ДобавитьСекцию("НачалоСекции");

            var countRealObj = 1;
            var totAreaMkd = 0M;
            var totAreaLivNotLivMkd = 0M;
            var totAreaLiv = 0M;
            var totAreaLivOwned = 0M;
            var totNumberEntrances = 0;
            var totNumberApartments = 0;
            var totNumberLifts = 0;
            var totNumberLiving = 0;
            var totAreaBasement = 0M;

            foreach (var realObjMo in realtyObjectCrByMo)
            {
                sectionMo.ДобавитьСтроку();
                var countRealObjByMo = 1;
                var areaMkd = 0M;
                var areaLivNotLivMkd = 0M;
                var areaLiv = 0M;
                var areaLivOwned = 0M;
                var numberEntrances = 0;
                var numberApartments = 0;
                var numberLifts = 0;
                var numberLiving = 0;
                var areaBasement = 0M;

                foreach (var realObj in realObjMo.Value)
                {
                    var nameProgramList = new List<string>();
                    var programCrNameList = string.Empty;
                    if (nameProgramCrDict.ContainsKey(realObj.Key))
                    {
                        nameProgramList = nameProgramCrDict[realObj.Key];
                        programCrNameList = nameProgramList.Aggregate((a, b) => a + ", " + b);
                    }
                    
                    var realObjList = realObj.Value[0];
                    section.ДобавитьСтроку();
                    section["Номер"] = countRealObj;
                    section["НомерПП"] = countRealObjByMo;
                    section["ДатаСоздания"] = realObjList.dateCreated;
                    section["ДатаРедактирования"] = realObjList.dateEdited;
                    section["ПрограммаКапРемонта"] = programCrNameList;
                    section["НаименованиеМО"] = realObjList.nameMo;
                    section["Адрес"] = realObjList.address;
                    section["Этажность"] = realObjList.floors;
                    section["Серия"] = realObjList.series;
                    section["ГруппаКапитальности"] = realObjList.groupCap;
                    section["ТипДома"] = realObjList.type;
                    section["СостояниеДома"] = realObjList.condition;
                    section["ОбщаяПлощадь"] = realObjList.areaMkd;
                    areaMkd += realObjList.areaMkd;
                    section["ОбщаяПлощадьЖилыхИНежилых"] = realObjList.areaLivNotLivMkd;
                    areaLivNotLivMkd += realObjList.areaLivNotLivMkd;
                    section["ВсегоЖилых"] = realObjList.areaLiv;
                    areaLiv += realObjList.areaLiv;
                    section["ВсегоЖилыхУГраждан"] = realObjList.areaLivOwned;
                    areaLivOwned += realObjList.areaLivOwned;
                    section["КолвоПодъездов"] = realObjList.numberEntrances;
                    numberEntrances += realObjList.numberEntrances;
                    section["КолвоКвартир"] = realObjList.numberApartments;
                    numberApartments += realObjList.numberApartments;
                    section["КолвоЛифтов"] = realObjList.numberLifts;
                    numberLifts += realObjList.numberLifts;
                    section["КолвоГраждан"] = realObjList.numberLiving;
                    numberLiving += realObjList.numberLiving;
                    section["НаличиеПодвала"] = realObjList.basement;
                    section["ПлощадьПодвала"] = realObjList.areaBasement;
                    areaBasement += realObjList.areaBasement;
                    section["СистемаОтопленния"] = realObjList.heatingSystem;
                    section["МатериалСтен"] = realObjList.wallMaterial;
                    section["МатериалКровли"] = realObjList.roofingMaterial;
                    section["ТипКровли"] = realObjList.typeRoof;
                    section["ГодВводаВЭксп"] = realObjList.dateCommissioning;
                    section["Износ"] = realObjList.physicalWear;
                    section["ГодПоследнегоКапРем"] = realObjList.dateLastOverhaul;


                    if (infoRealtyObjByGisu[realObjMo.Key].ContainsKey(realObj.Key))
                    {
                        section["ДоговорСГИСУ"] = infoRealtyObjByGisu[realObjMo.Key][realObj.Key][0].numDoc + ", "
                                                  + infoRealtyObjByGisu[realObjMo.Key][realObj.Key][0].dateDoc.ToShortDateString();
                    }
                    else
                    {
                        section["ДоговорСГИСУ"] = string.Empty;
                    }

                    if (infoManorgByReality[realObjMo.Key].ContainsKey(realObj.Key))
                    {
                        var typeName = infoManorgByReality[realObjMo.Key][realObj.Key].Select(x => x.typeManag).ToList();
                        var name = infoManorgByReality[realObjMo.Key][realObj.Key].Select(x => x.nameManOrg).ToList();

                        var typeNameList = typeName.Aggregate((a, b) => a + ", " + b);
                        var nameList = name.Aggregate((a, b) => a + ", " + b);

                        section["СпособУправленияМКД"] = typeNameList;
                        section["НаименованиеУпрОрг"] = nameList;
                    }
                    else
                    {
                        section["СпособУправленияМКД"] = string.Empty;
                        section["НаименованиеУпрОрг"] = string.Empty;
                    }

                    if (infoDeviceByReality.ContainsKey(realObjMo.Key) && infoDeviceByReality[realObjMo.Key].Contains(realObj.Key))
                    {
                        section["ПриборыУчёта"] = "Да";
                    }
                    else
                    {
                        section["ПриборыУчёта"] = "Нет";
                    }

                    ++countRealObj;
                    ++countRealObjByMo;
                }

                sectionMo["названиеМО"] = municipalityDict[realObjMo.Key];
                sectionMo["ОбщаяПлощадьМО"] = areaMkd;
                totAreaMkd += areaMkd;
                sectionMo["ОбщаяПлощадьЖилыхИНежилыхМО"] = areaLivNotLivMkd;
                totAreaLivNotLivMkd += areaLivNotLivMkd;
                sectionMo["ВсегоЖилыхМО"] = areaLiv;
                totAreaLiv += areaLiv;
                sectionMo["ВсегоЖилыхУГражданМО"] = areaLivOwned;
                totAreaLivOwned += areaLivOwned;
                sectionMo["КолвоПодъездовМО"] = numberEntrances;
                totNumberEntrances += numberEntrances;
                sectionMo["КолвоКвартирМО"] = numberApartments;
                totNumberApartments += numberApartments;
                sectionMo["КолвоЛифтовМО"] = numberLifts;
                totNumberLifts += numberLifts;
                sectionMo["КолвоГражданМО"] = numberLiving;
                totNumberLiving += numberLiving;
                sectionMo["ПлощадьПодвалаМО"] = areaBasement;
                totAreaBasement += areaBasement;
            }

            reportParams.SimpleReportParams["ОбщаяПлощадь"] = totAreaMkd;
            reportParams.SimpleReportParams["ОбщаяПлощадьЖилыхИНежилых"] = totAreaLivNotLivMkd;
            reportParams.SimpleReportParams["ВсегоЖилых"] = totAreaLiv;
            reportParams.SimpleReportParams["ВсегоЖилыхУГраждан"] = totAreaLivOwned;
            reportParams.SimpleReportParams["КолвоПодъездов"] = totNumberEntrances;
            reportParams.SimpleReportParams["КолвоКвартир"] = totNumberApartments;
            reportParams.SimpleReportParams["КолвоЛифтов"] = totNumberLifts;
            reportParams.SimpleReportParams["КолвоГраждан"] = totNumberLiving;
            reportParams.SimpleReportParams["ПлощадьПодвала"] = totAreaBasement;
        }
    }
}