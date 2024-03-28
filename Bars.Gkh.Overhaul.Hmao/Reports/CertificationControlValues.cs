namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Properties;

    using Castle.Windsor;

    class CertificationControlValues : BasePrintForm
    {
        public CertificationControlValues()
            : base(new ReportTemplateBinary(Resources.CertificationControlValues))
        {
        }

        private long[] municipalityIds;

        private int[] houseTypes;

        private int[] conditionHouses;

        public IWindsorContainer Container { get; set; }

        public IDomainService<Municipality> MoDomainService { get; set; }


        public override string Name
        {
            get { return "Контроль паспортизации домов (значения)"; }
        }

        public override string Desciption
        {
            get { return "Контроль паспортизации домов (значения)"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.CertificationControlValues";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.CertificationControlValues";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            var houseTypesList = baseParams.Params.GetAs("houseTypes", string.Empty);
            houseTypes = !string.IsNullOrEmpty(houseTypesList)
                                  ? houseTypesList.Split(',').Select(id => id.ToInt()).ToArray()
                                  : new int[0];

            var conditionHousesList = baseParams.Params.GetAs("conditionHouses", string.Empty);
            conditionHouses = !string.IsNullOrEmpty(conditionHousesList)
                                  ? conditionHousesList.Split(',').Select(id => id.ToInt()).ToArray()
                                  : new int[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var publishedProgramRecordDomain = Container.ResolveDomain<PublishedProgramRecord>();
            var realityObjectDomain = Container.ResolveDomain<RealityObject>();

            using (this.Container.Using(publishedProgramRecordDomain, realityObjectDomain))
            {
                var housePublished = publishedProgramRecordDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .Select(x => x.RealityObject);

                var realtyObjects = realityObjectDomain.GetAll()
                    .Where(x => housePublished.Any(y => y == x))
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                    .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int) x.TypeHouse))
                    .WhereIf(conditionHouses.Length > 0, x => conditionHouses.Contains((int) x.ConditionHouse))
                    .Where(x => !x.IsNotInvolvedCr)
                    .Select(x => new RoProxy
                    {
                        muName = x.Municipality.Name,
                        Id = x.Id,
                        Address = x.Address,
                        TypeHouse = x.TypeHouse,
                        DateCommissioning = x.DateCommissioning,
                        BuildYear = x.BuildYear,
                        AreaMkd = x.AreaMkd,
                        AreaOwned = x.AreaOwned,
                        AreaLiving = x.AreaLiving,
                        AreaLivingOwned = x.AreaLivingOwned,
                        AreaMunicipalOwned = x.AreaMunicipalOwned,
                        AreaNotLivingFunctional = x.AreaNotLivingFunctional,
                        AreaGovernmentOwned = x.AreaGovernmentOwned,
                        AreaCommonUsage = x.AreaCommonUsage,
                        Floors = x.Floors,
                        MaximumFloors = x.MaximumFloors,
                        NumberEntrances = x.NumberEntrances,
                        NumberLiving = x.NumberLiving,
                        PhysicalWear = x.PhysicalWear,
                        PrivatizationDateFirstApartment = x.PrivatizationDateFirstApartment,
                        IsRepairInadvisable = x.IsRepairInadvisable,
                        NumberApartments = x.NumberApartments,
                        AreaLivingNotLivingMkd = x.AreaLivingNotLivingMkd,
                        MoSettlement = x.MoSettlement != null ? x.MoSettlement.Name : "",
                        IsIncludedRegisterCHO = GetYesNoValue(x.IsIncludedRegisterCHO),
                        IsIncludedListIdentifiedCHO = GetYesNoValue(x.IsIncludedListIdentifiedCHO),
                        IsDeterminedSubjectProtectionCHO = GetYesNoValue(x.IsDeterminedSubjectProtectionCHO)
                    })
                    .OrderBy(x => x.muName)
                    .ThenBy(x => x.Address)
                    .ToList();

                var realityObjectStructuralElementDomain = Container.ResolveDomain<RealityObjectStructuralElement>();
                using (this.Container.Using(realityObjectStructuralElementDomain))
                {
                    var realityObjectStructuralElementsDict = realityObjectStructuralElementDomain.GetAll()
                        .Where(x => housePublished.Any(y => y == x.RealityObject))
                        .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                        .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int) x.RealityObject.TypeHouse))
                        .WhereIf(conditionHouses.Length > 0, x => conditionHouses.Contains((int) x.RealityObject.ConditionHouse))
                        .Where(x => !x.RealityObject.IsNotInvolvedCr)
                        .Where(x => x.StructuralElement.Group.CommonEstateObject.IncludedInSubjectProgramm)
                        .Where(x => x.StructuralElement.Group.UseInCalc)
                        .Where(x => x.State.StartState)
                        .Select(x => new
                        {
                            roId = x.RealityObject.Id,
                            x.Name,
                            StructName = x.StructuralElement.Name,
                            x.Volume,
                            x.LastOverhaulYear,
                            x.Wearout,
                            x.StructuralElement.Group.CommonEstateObject.MultipleObject
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.roId)
                        .ToDictionary(x => x.Key);

                    var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
                    var num = 0;

                    foreach (var realtyObject in realtyObjects)
                    {
                        num++;

                        if (!realityObjectStructuralElementsDict.ContainsKey(realtyObject.Id))
                        {
                            section.ДобавитьСтроку();

                            FillRealObjInfo(section, realtyObject, num);

                            continue;
                        }

                        foreach (var realityObjectStructuralElement in realityObjectStructuralElementsDict[realtyObject.Id])
                        {
                            section.ДобавитьСтроку();

                            FillRealObjInfo(section, realtyObject, num);

                            if (realityObjectStructuralElement.MultipleObject)
                            {
                                section["seName"] = realityObjectStructuralElement.Name;
                            }
                            else
                            {
                                section["seName"] = realityObjectStructuralElement.StructName;
                            }

                            section["seVolume"] = realityObjectStructuralElement.Volume;
                            section["seLastOverhaulYear"] = realityObjectStructuralElement.LastOverhaulYear;
                            section["seWearout"] = realityObjectStructuralElement.Wearout;
                        }
                    }
                }
            }
        }

        private void FillRealObjInfo(Section section, RoProxy realtyObject, int num)
        {

            section["Num"] = num;
            section["MuName"] = realtyObject.muName;
            section["AD"] = realtyObject.MoSettlement;
            section["Address"] = realtyObject.Address;
            section["TypeHouse"] = realtyObject.TypeHouse.GetEnumMeta().Display;
            section["DateCommissioning"] = realtyObject.DateCommissioning.HasValue ? realtyObject.DateCommissioning.Value.ToString("dd.MM.yyyy") : string.Empty;
            section["BuildYear"] = realtyObject.BuildYear;
            section["IsIncludedRegisterCHO"] = realtyObject.IsIncludedRegisterCHO;
            section["IsIncludedListIdentifiedCHO"] = realtyObject.IsIncludedListIdentifiedCHO;
            section["IsDeterminedSubjectProtectionCHO"] = realtyObject.IsDeterminedSubjectProtectionCHO;
            section["AreaMkd"] = realtyObject.AreaMkd;
            section["AreaOwned"] = realtyObject.AreaOwned;
            section["AreaMunicipalOwned"] = realtyObject.AreaMunicipalOwned;
            section["AreaGovernmentOwned"] = realtyObject.AreaGovernmentOwned;
            section["AreaLivingOwned"] = realtyObject.AreaLivingOwned;
            section["AreaNotLivingFunctional"] = realtyObject.AreaNotLivingFunctional;
            section["AreaCommonUsage"] = realtyObject.AreaCommonUsage;
            section["IsRepairInadvisable"] = realtyObject.IsRepairInadvisable ? "да" : "нет";
            section["AreaLiving"] = realtyObject.AreaLiving;
            section["Floors"] = realtyObject.Floors;
            section["MaximumFloors"] = realtyObject.MaximumFloors;
            section["NumberEntrances"] = realtyObject.NumberEntrances;
            section["NumberLiving"] = realtyObject.NumberLiving;
            section["PhysicalWear"] = realtyObject.PhysicalWear;
            section["NumberApartments"] = realtyObject.NumberApartments;
            section["AreaLivingNotLivingMkd"] = realtyObject.AreaLivingNotLivingMkd;
            section["PrivatizationDate"] = realtyObject.PrivatizationDateFirstApartment.HasValue
                     ? realtyObject.PrivatizationDateFirstApartment.Value.ToString("dd.MM.yyyy")
                     : string.Empty;
        }

        private class RoProxy
        {
            public string muName;
            public long Id;
            public string Address;
            public decimal? AreaOwned;
            public TypeHouse TypeHouse;
            public DateTime? DateCommissioning;
            public int? BuildYear;
            public decimal? AreaMkd;
            public decimal? AreaLiving;
            public decimal? AreaLivingOwned;
            public decimal? AreaMunicipalOwned;
            public decimal? AreaNotLivingFunctional;
            public decimal? AreaGovernmentOwned;
            public decimal? AreaCommonUsage;
            public int? Floors;
            public int? MaximumFloors;
            public int? NumberEntrances;
            public int?  NumberLiving;
            public decimal? PhysicalWear;
            public DateTime? PrivatizationDateFirstApartment;
            public bool IsRepairInadvisable;
            public int? NumberApartments;
            public decimal? AreaLivingNotLivingMkd;
            public string MoSettlement;
            public string IsIncludedRegisterCHO;
            public string IsIncludedListIdentifiedCHO;
            public string IsDeterminedSubjectProtectionCHO;
        }
        private string GetYesNoValue(bool? nullableBool)
        {
            return nullableBool.HasValue
                ? nullableBool.Value
                    ? "Да"
                    : "Нет"
                : "";
        }
    }
}


