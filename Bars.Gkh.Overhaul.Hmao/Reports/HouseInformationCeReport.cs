namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Properties;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Информация по домам
    /// </summary>
    public class HouseInformationCeReport : BasePrintForm
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public HouseInformationCeReport()
            : base(new ReportTemplateBinary(Resources.HouseInformation))
        {
        }

        private long[] municipalityIds;
        private long[] ceoIds;
        private int[] houseTypes;
        private int housesList;
        private bool isMain;

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Жилой дом договора управляющей организации
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> ManOrgBaseContractService { get; set; }

        /// <summary>
        /// Конструктивный элемент дома
        /// </summary>
        public IDomainService<RealityObjectMissingCeo> RealityObjectMissingCeoService { get; set; }

        /// <summary>
        /// Запись Опубликованной программы
        /// </summary>
        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }

        /// <summary>
        /// Запись версии программы
        /// </summary>
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        /// <summary>
        ///  Конструктивный элемент дома
        /// </summary>
        public IDomainService<RealityObjectStructuralElement> RealityObjectStructuralElementDomain { get; set; }

        /// <summary>
        ///  Конструктивный элемент
        /// </summary>
        public IDomainService<StructuralElement> StructuralElementDomain { get; set; }

        /// <summary>
        /// Сервис модификации коллекции
        /// </summary>
        public IModifyEnumerableService ModifyEnumerableService { get; set; }

        /// <inheritdoc />
        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];

            var ceoIdsList = baseParams.Params.GetAs("ceoIds", string.Empty);
            this.ceoIds = !string.IsNullOrEmpty(ceoIdsList)
                ? ceoIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];

            var houseTypesList = baseParams.Params.GetAs("houseTypes", string.Empty);
            this.houseTypes = !string.IsNullOrEmpty(houseTypesList)
                ? houseTypesList.Split(',').Select(id => id.ToInt()).ToArray()
                : new int[0];

            this.housesList = baseParams.Params.GetAs<int>("housesList");

            this.isMain = baseParams.Params.GetAs<bool>("cbMain");
        }

        /// <inheritdoc />
        public override string Name
        {
            get { return "Информация по домам"; }
        }

        /// <inheritdoc />
        public override string Desciption
        {
            get { return "Информация по домам"; }
        }

        /// <inheritdoc />
        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        /// <inheritdoc />
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.HouseInformationCeReport";
            }
        }

        /// <inheritdoc />
        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.HouseInformationCeReport";
            }
        }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            long[] realObjectIds = null;

            if (this.housesList == 2)
            {
                //переносим отчет на версию программы
                //realObjectIds = this.PublishedProgramRecordDomain.GetAll()
                //    .WhereIf(isMain, x => x.PublishedProgram.ProgramVersion.IsMain)
                //    .Select(x => x.RealityObject.Id)
                //    .Distinct()
                //    .ToArray();

                realObjectIds = this.VersionRecordDomain.GetAll()
                   .WhereIf(isMain, x => x.ProgramVersion.IsMain)
                   .Select(x => x.RealityObject.Id)
                   .Distinct()
                   .ToArray();
                

            }

            var realtyObjects = this.RealityObjectRepository.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Municipality.Id) || this.municipalityIds.Contains(x.MoSettlement.Id))
                .WhereIf(this.houseTypes.Length > 0, x => this.houseTypes.Contains((int)x.TypeHouse))
                .WhereIf(realObjectIds != null, x => realObjectIds.Contains(x.Id))
                .Select(x => new RealityObjectProxy
                {
                    MuName = x.Municipality.Name,
                    Id = x.Id,
                    Address = x.Address,
                    TypeHouse = x.TypeHouse.GetEnumMeta().Display,
                    DateCommissioning = x.DateCommissioning,
                    BuildYear = x.BuildYear,
                    AreaMkd = x.AreaMkd,
                    AreaOwned = x.AreaOwned,
                    AreaLiving = x.AreaLiving,
                    AreaLivingNotLivingMkd = x.AreaLivingNotLivingMkd,
                    AreaLivingOwned = x.AreaLivingOwned,
                    AreaMunicipalOwned = x.AreaMunicipalOwned,
                    AreaNotLivingFunctional = x.AreaNotLivingFunctional,
                    AreaGovernmentOwned = x.AreaGovernmentOwned,
                    AreaCommonUsage = x.AreaCommonUsage,
                    Floors = x.Floors,
                    MaximumFloors = x.MaximumFloors,
                    NumberEntrances = x.NumberEntrances,
                    NumberApartments = x.NumberApartments,
                    NumberLiving = x.NumberLiving,
                    PhysicalWear = x.PhysicalWear,
                    IsSubProgram = x.IsSubProgram? "Подпрограмма": "Основная программа",
                    PrivatizationDateFirstApartment = x.PrivatizationDateFirstApartment,
                    IsRepairInadvisable = x.IsRepairInadvisable,
                    ManOrgName = x.ManOrgs
                })
                .OrderBy(x => x.MuName)
                .ThenBy(x => x.Address)
                .ToList();

            if (this.ModifyEnumerableService != null)
            {
                realtyObjects = this.ModifyEnumerableService.ReplaceProperty(realtyObjects, ".", x => x.MuName, x => x.Address).ToList();
            }

            var realityObjectStructuralElementsDict = this.RealityObjectStructuralElementDomain.GetAll()
                .Where(x => x.State.StartState)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(this.houseTypes.Length > 0, x => this.houseTypes.Contains((int)x.RealityObject.TypeHouse))
                .WhereIf(this.ceoIds.Any(), x => this.ceoIds.Contains(x.StructuralElement.Group.CommonEstateObject.Id))
                .Select(x => new
                {
                    elemId = x.StructuralElement.Id,
                    roId = x.RealityObject.Id,
                    Name = x.Name ?? x.StructuralElement.Name,
                    GroupName = x.StructuralElement.Group.Name,
                    ceoName = x.StructuralElement.Group.CommonEstateObject.Name,
                    ceoId = x.StructuralElement.Group.CommonEstateObject.Id,
                    x.Volume,
                    x.LastOverhaulYear,
                    x.Wearout
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key);

            var structuralElementsDict = this.StructuralElementDomain.GetAll()
               .WhereIf(this.ceoIds.Any(), x => this.ceoIds.Contains(x.Group.CommonEstateObject.Id))
               .Select(x => new
               {
                   x.Id,
                   Name = x.Name ?? x.Name,
                   x.Group.CommonEstateObject,
                   GroupName = x.Group.Name
               })
               .AsEnumerable()
               .GroupBy(x => x.CommonEstateObject)
               .OrderBy(x => x.Key.Name)
               .ToDictionary(x => x.Key);

            var ceIds = new HashSet<long>();
            var ceIds2 = new HashSet<long>();

            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("vertSection");
            var verticalSection2 = reportParams.ComplexReportParams.ДобавитьСекцию("vertSection2");

            foreach (var group in structuralElementsDict.Keys)
            {
                foreach (var elem in structuralElementsDict[group])
                {
                    // на лист влезет не более 75*3 колонок, остальное на лист2
                    if (ceIds.Count <= 75)
                    {
                        verticalSection.ДобавитьСтроку();
                        verticalSection["groupname"] = group.Name;
                        verticalSection["elemName"] = elem.Name;

                        verticalSection["year"] = $"$year{elem.Id}$";
                        verticalSection["wear"] = $"$wear{elem.Id}$";
                        verticalSection["volume"] = $"$volume{elem.Id}$";

                        ceIds.Add(elem.Id);
                    }
                    else if (ceIds2.Count < 155)
                    {
                        verticalSection2.ДобавитьСтроку();
                        verticalSection2["groupname"] = group.Name;
                        verticalSection2["elemName"] = elem.Name;

                        verticalSection2["year"] = $"$year{elem.Id}$";
                        verticalSection2["wear"] = $"$wear{elem.Id}$";
                        verticalSection2["volume"] = $"$volume{elem.Id}$";

                        ceIds2.Add(elem.Id);
                    }
                }
            }

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            Section section2 = null;
            if (ceIds2.Any())
            {
                section2 = reportParams.ComplexReportParams.ДобавитьСекцию("section2");
            }

            var num = 0;

            foreach (var realtyObject in realtyObjects)
            {
                section.ДобавитьСтроку();
                if (section2 != null)
                {
                    section2.ДобавитьСтроку();
                }

                Dictionary<string, string[]> dictTPV = new Dictionary<string, string[]>();
                dictTPV.Add("Place", new string[2] { "Form_1", "16:1" });
                dictTPV.Add("AccountsCount", new string[2] { "Form_1", "14:1" });
                dictTPV.Add("KadastrNum", new string[2] { "Form_1", "30:1" });
                var tpvContainer = Container.Resolve<IDomainService<TehPassportValue>>();
               

                section["Num"] = ++num;
                section["MuName"] = realtyObject.MuName;
                section["Address"] = realtyObject.Address;
                section["TypeHouse"] = realtyObject.TypeHouse;
                section["DateCommissioning"] = realtyObject.DateCommissioning.HasValue
                    ? realtyObject.DateCommissioning.Value.ToString("dd.MM.yyyy")
                    : string.Empty;
                section["BuildYear"] = realtyObject.BuildYear;
                section["AreaMkd"] = realtyObject.AreaMkd;
                section["AreaOwned"] = realtyObject.AreaOwned;
                foreach (KeyValuePair<string, string[]> dictkey in dictTPV)
                {
                    section[dictkey.Key] = tpvContainer.GetAll()
                .FirstOrDefault(x => x.TehPassport.RealityObject.Id == realtyObject.Id && x.FormCode == dictkey.Value[0] && x.CellCode == dictkey.Value[1]);
                }
                section["AreaMunicipalOwned"] = realtyObject.AreaMunicipalOwned;
                section["AreaGovernmentOwned"] = realtyObject.AreaGovernmentOwned;
                section["AreaLiving"] = realtyObject.AreaLiving;
                section["AreaLivingNotLivingMkd"] = realtyObject.AreaLivingNotLivingMkd;
                section["AreaLivingOwned"] = realtyObject.AreaLivingOwned;
                section["AreaNotLivingFunctional"] = realtyObject.AreaNotLivingFunctional;
                section["AreaCommonUsage"] = realtyObject.AreaCommonUsage;
                section["Floors"] = realtyObject.Floors;
                section["MaximumFloors"] = realtyObject.MaximumFloors;
                section["NumberEntrances"] = realtyObject.NumberEntrances;
                section["NumberLiving"] = realtyObject.NumberLiving;
                section["ProgramCR"] = realtyObject.IsSubProgram;
                section["NumberApartments"] = realtyObject.NumberApartments;
                section["PhysicalWear"] = realtyObject.PhysicalWear;
                section["IsRepairInadvisable"] = realtyObject.IsRepairInadvisable ? "да" : "нет";
                section["ManOrgName"] = realtyObject.ManOrgName;

                section["PrivatizationDate"] = realtyObject.PrivatizationDateFirstApartment.HasValue
                    ? realtyObject.PrivatizationDateFirstApartment.Value.ToString("dd.MM.yyyy")
                    : string.Empty;

                foreach (var ceo in structuralElementsDict.Keys)
                {
                    foreach (var elem in structuralElementsDict[ceo])
                    {
                        if (realityObjectStructuralElementsDict.ContainsKey(realtyObject.Id))
                        {
                            if (realityObjectStructuralElementsDict[realtyObject.Id].Any(x => x.elemId == elem.Id))
                            {
                                var realityObjectStructuralElement =
                                    realityObjectStructuralElementsDict[realtyObject.Id].FirstOrDefault(
                                        x => x.elemId == elem.Id);

                                if (ceIds.Contains(realityObjectStructuralElement.elemId))
                                {
                                    section[$"volume{realityObjectStructuralElement.elemId}"] =
                                        realityObjectStructuralElement.Volume;
                                    section[$"year{realityObjectStructuralElement.elemId}"] =
                                        realityObjectStructuralElement.LastOverhaulYear;
                                    section[$"wear{realityObjectStructuralElement.elemId}"] =
                                        realityObjectStructuralElement.Wearout;
                                }
                                else if (ceIds2.Contains(realityObjectStructuralElement.elemId))
                                {
                                    section2[$"volume{realityObjectStructuralElement.elemId}"] =
                                        realityObjectStructuralElement.Volume;
                                    section2[$"year{realityObjectStructuralElement.elemId}"] =
                                        realityObjectStructuralElement.LastOverhaulYear;
                                    section2[$"wear{realityObjectStructuralElement.elemId}"] =
                                        realityObjectStructuralElement.Wearout;
                                }
                            }
                        }
                    }
                }
            }
        }

        private class RealityObjectProxy
        {
            public string MuName { get; set; }
            public long Id { get; set; }
            public string Address { get; set; }
            public string IsSubProgram { get; set; }
            public string TypeHouse { get; set; }
            public DateTime? DateCommissioning { get; set; }
            public int? BuildYear { get; set; }
            public decimal? AreaMkd { get; set; }
            public decimal? AreaOwned { get; set; }
            public decimal? AreaLiving { get; set; }
            public decimal? AreaLivingNotLivingMkd { get; set; }
            public decimal? AreaLivingOwned { get; set; }
            public decimal? AreaMunicipalOwned { get; set; }
            public decimal? AreaNotLivingFunctional { get; set; }
            public decimal? AreaGovernmentOwned { get; set; }
            public decimal? AreaCommonUsage { get; set; }
            public int? Floors { get; set; }
            public int? MaximumFloors { get; set; }
            public int? NumberEntrances { get; set; }
            public int? NumberApartments { get; set; }
            public int? NumberLiving { get; set; }
            public decimal? PhysicalWear { get; set; }
            public DateTime? PrivatizationDateFirstApartment { get; set; }
            public bool IsRepairInadvisable { get; set; }
            public string ManOrgName { get; set; }
        }
    }
}