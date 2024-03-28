namespace Bars.Gkh.Overhaul.Nso.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Nso.Properties;

    using Castle.Windsor;

    public class CertificationControlValuesWithQuality : BasePrintForm
    {
        public CertificationControlValuesWithQuality()
            : base(new ReportTemplateBinary(Resources.CertificationControlValuesWithQuality))
        {
        }

        private long[] municipalityIds;

        private int[] houseTypes;

        private int[] conditionHouses;

        public IWindsorContainer Container { get; set; }

        public IDomainService<Municipality> MoDomainService { get; set; }


        public override string Name
        {
            get { return "Контроль паспортизации домов (значения с качеством заполнения данных)"; }
        }

        public override string Desciption
        {
            get { return "Контроль паспортизации домов (значения с качеством заполнения данных)"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.CertificationControlValuesWithQuality";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.CertificationControlValuesWithQuality";
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

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
        public override void PrepareReport(ReportParams reportParams)
        {
            var query =
                Container.Resolve<IDomainService<RealityObject>>()
                    .GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                    .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.TypeHouse))
                    .WhereIf(conditionHouses.Length > 0, x => conditionHouses.Contains((int)x.ConditionHouse))
                    .Where(x => !x.IsNotInvolvedCr)
                    .Select(
                        x =>
                        new RoProxy
                        {
                            MunicipalityId = x.Municipality.Id,
                            MunicipalityName = x.Municipality.Name,
                            muName = x.Municipality.Name,
                            Id = x.Id,
                            Address = x.Address,
                            TypeHouse = x.TypeHouse,
                            DateCommissioning = x.DateCommissioning,
                            BuildYear = x.BuildYear,
                            AreaMkd = x.AreaMkd,
                            AreaLiving = x.AreaLiving,
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
                            HasPrivatizedFlats = x.HasPrivatizedFlats,
                            NecessaryConductCr = x.NecessaryConductCr,
                            TypeProject = x.TypeProject.Name,
                            CadastreNumber = x.CadastreNumber
                        }).ToArray();

            var realityObjectIds = query.Select(x => x.Id).ToArray();

            var roGroupnigList =
                query.GroupBy(x => new { x.MunicipalityId, x.MunicipalityName })
                    .ToDictionary(x => x.Key, y => y.OrderBy(x => x.muName).ThenBy(x => x.Address));

            var realityObjectStructuralElementsDict =
                Container.Resolve<IDomainService<RealityObjectStructuralElement>>()
                    .GetAll()
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                    .WhereIf(
                        conditionHouses.Length > 0,
                        x => conditionHouses.Contains((int)x.RealityObject.ConditionHouse))
                    .Where(x => !x.RealityObject.IsNotInvolvedCr)
                    .Where(x => x.StructuralElement.Group.CommonEstateObject.IncludedInSubjectProgramm)
                    .Where(x => x.StructuralElement.Group.UseInCalc)
                    .Where(x => x.State.StartState)
                    .Select(
                        x =>
                        new
                        {
                            roId = x.RealityObject.Id,
                            x.Name,
                            StructName = x.StructuralElement.Name,
                            x.Volume,
                            x.LastOverhaulYear,
                            x.Wearout,
                            x.StructuralElement.Group.Required,
                            x.StructuralElement.Group.CommonEstateObject,
                            x.StructuralElement.Group.CommonEstateObject.MultipleObject
                        })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");


            var realityObjectRoomsQuality =
                Container.Resolve<IDomainService<Room>>()
                    .GetAll()
                    .Where(x => realityObjectIds.Contains(x.RealityObject.Id))
                    .Select(
                        x =>
                        new
                        {
                            roId = x.RealityObject.Id,
                            x.RoomNum,
                            Area = x.Area != 0m,
                            LivingArea = x.LivingArea != 0m,
                            RoomOwnershipType = x.OwnershipType != RoomOwnershipType.NotSet
                        })
                    .ToArray()
                    .Select(
                        x =>
                        new
                        {
                            x.roId,
                            RoomNum = !string.IsNullOrEmpty(x.RoomNum),
                            x.Area,
                            x.LivingArea,
                            x.RoomOwnershipType
                        })
                    .GroupBy(x => x.roId)
                    .ToDictionary(
                        x => x.Key,
                        y =>
                        new
                        {
                            RoomNum = y.All(x => x.RoomNum),
                            Area = y.All(x => x.Area),
                            LivingArea = y.All(x => x.LivingArea),
                            RoomOwnershipType = y.All(x => x.RoomOwnershipType),
                        });

            var missingList =
                Container.Resolve<IDomainService<RealityObjectMissingCeo>>()
                    .GetAll()
                    .Where(x => realityObjectIds.Contains(x.RealityObject.Id))
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, y => y.Count());

            var result = new List<Row>();
            var num = 0;
            var lastNum = 0;

            double apartments = 0;
            double main = 0;
            double estate = 0;
            double additional = 0;

            // Группировка по муниципальному району
            foreach (var pair in roGroupnigList)
            {
                double moQualityApartments = 0;
                double moQualityMain = 0;
                double moQualityEstate = 0;
                double moQualityAdditional = 0;

                var moRow = new Row { muName = pair.Key.MunicipalityName };

                // По жилому дому
                foreach (var realtyObject in pair.Value)
                {
                    num++;

                    var roRow = new Row(realtyObject) { num = num };

                    result.Add(roRow);

                    // Качество заполнения для помещений
                    if (realityObjectRoomsQuality.ContainsKey(realtyObject.Id))
                    {
                        var obj = realityObjectRoomsQuality[realtyObject.Id];

                        // 1 свойство всегда заполнено
                        double qualityApartments = 1;
                        ProcessQuality(obj.RoomNum, ref qualityApartments);
                        ProcessQuality(obj.Area, ref qualityApartments);
                        ProcessQuality(obj.LivingArea, ref qualityApartments);
                        ProcessQuality(obj.RoomOwnershipType, ref qualityApartments);
                        roRow.QualityApartments = (qualityApartments / 5.0) * 100;
                        moQualityApartments += roRow.QualityApartments;
                    }

                    if (!realityObjectStructuralElementsDict.ContainsKey(realtyObject.Id))
                    {
                        continue;
                    }

                    // Качество заполнения основных данных
                    double qualityMain = 0;
                    this.ProcessQuality(!string.IsNullOrEmpty(realtyObject.Address), ref qualityMain);
                    this.ProcessQuality(realtyObject.DateCommissioning.HasValue, ref qualityMain);
                    this.ProcessQuality(realtyObject.BuildYear.HasValue, ref qualityMain);
                    this.ProcessQuality(realtyObject.HasPrivatizedFlats.HasValue, ref qualityMain);
                    this.ProcessQuality(realtyObject.PrivatizationDateFirstApartment.HasValue, ref qualityMain);
                    this.ProcessQuality(realtyObject.NumberApartments.HasValue, ref qualityMain);
                    this.ProcessQuality(realtyObject.AreaMkd.HasValue, ref qualityMain);
                    this.ProcessQuality(realtyObject.AreaLiving.HasValue, ref qualityMain);
                    this.ProcessQuality(realtyObject.NecessaryConductCr != YesNoNotSet.NotSet, ref qualityMain);
                    this.ProcessQuality(realtyObject.TypeHouse != TypeHouse.NotSet, ref qualityMain);

                    roRow.QualityMain = (qualityMain / 10.0) * 100;

                    moQualityMain += roRow.QualityMain;

                    // Качество заполнения дополнительных данных
                    double qualityAdditional = 0;

                    this.ProcessQuality(!string.IsNullOrEmpty(realtyObject.TypeProject), ref qualityAdditional);
                    this.ProcessQuality(!string.IsNullOrEmpty(realtyObject.CadastreNumber), ref qualityAdditional);
                    this.ProcessQuality(realtyObject.PhysicalWear.HasValue, ref qualityAdditional);
                    this.ProcessQuality(realtyObject.AreaLivingNotLivingMkd.HasValue, ref qualityAdditional);
                    this.ProcessQuality(realtyObject.MaximumFloors.HasValue, ref qualityAdditional);
                    var a = (qualityAdditional / 5.0) * 100;

                    roRow.QualityAdditional = (qualityAdditional / 5.0) * 100;

                    moQualityAdditional += roRow.QualityAdditional;

                    var requiredCount = 0;

                    // По ООИ
                    foreach (var realityObjectStructuralElementGroup in
                        realityObjectStructuralElementsDict[realtyObject.Id].GroupBy(x => x.CommonEstateObject))
                    {
                        result.Add(
                            new Row(realtyObject)
                            {
                                QualityEstate = 100,
                                CommonEstateObject = realityObjectStructuralElementGroup.Key.Name,
                                num = num,
                                isCommonEstateElement = true
                            });

                        // По КЭ
                        foreach (var realityObjectStructuralElement in realityObjectStructuralElementGroup)
                        {
                            if (realityObjectStructuralElement.Required)
                            {
                                requiredCount++;
                            }

                            result.Add(
                                new Row(realtyObject)
                                {
                                    seName =
                                        realityObjectStructuralElement.MultipleObject
                                            ? realityObjectStructuralElement.Name
                                            : realityObjectStructuralElement.StructName,
                                    seVolume = realityObjectStructuralElement.Volume,
                                    seLastOverhaulYear = realityObjectStructuralElement.LastOverhaulYear,
                                    seWearout = realityObjectStructuralElement.Wearout,

                                    // Тут все всегда заполнено
                                    QualityEstate = 100,
                                    CommonEstateObject = realityObjectStructuralElementGroup.Key.Name,
                                    isStructuralElement = true,
                                    num = num
                                });
                        }
                    }

                    // Качество заполнения ООИ
                    if (requiredCount != 0 && missingList.ContainsKey(realtyObject.Id)
                        && requiredCount > missingList[realtyObject.Id])
                    {
                        roRow.QualityEstate = (requiredCount - missingList[realtyObject.Id]) * 100;
                    }
                    else
                    {
                        roRow.QualityEstate = 0;
                    }

                    moQualityEstate += roRow.QualityEstate;
                    roRow.QualityCommon = (roRow.QualityApartments * 0.8 + roRow.QualityEstate * 1.4
                                           + roRow.QualityMain * 1.4 + roRow.QualityAdditional * 0.4) / 4;
                }

                moRow.QualityEstate = moQualityEstate / (num - lastNum);
                moRow.QualityApartments = moQualityApartments / (num - lastNum);
                moRow.QualityMain = moQualityMain / (num - lastNum);
                moRow.QualityAdditional = moQualityAdditional / (num - lastNum);
                moRow.QualityCommon = (apartments * 0.8 + estate * 1.4 + main * 1.4 + additional * 0.4) / 4;

                lastNum = num;

                apartments += moQualityApartments;
                estate += moQualityEstate;
                additional += moQualityAdditional;
                main += moQualityMain;
            }

            var avMain = main / num;
            var avEstate = estate / num;
            var avAdditional = additional / num;
            var avApartments = apartments / num;

            section.ДобавитьСтроку();
            section["QualityApartments"] = avApartments;
            section["QualityAdditional"] = avAdditional;
            section["QualityEstate"] = avEstate;
            section["QualityMain"] = avMain;
            section["QualityCommon"] = (avApartments * 0.8 + avEstate * 1.4 + avMain * 1.4 + avAdditional * 0.4) / 4;

            foreach (var row in result)
            {
                FillRealObjInfo(section, row);
            }
        }

        private void FillRealObjInfo(Section section, Row realtyObject)
        {
            section.ДобавитьСтроку();

            section["Num"] = realtyObject.num;
            section["MuName"] = realtyObject.muName;
            section["AD"] = realtyObject.MoSettlement;
            section["Address"] = realtyObject.Address;
            section["TypeHouse"] = realtyObject.TypeHouse.GetEnumMeta().Display;
            section["DateCommissioning"] = realtyObject.DateCommissioning.HasValue
                                               ? realtyObject.DateCommissioning.Value.ToString("dd.MM.yyyy")
                                               : string.Empty;
            section["BuildYear"] = realtyObject.BuildYear;
            section["AreaMkd"] = realtyObject.AreaMkd;
            section["AreaLiving"] = realtyObject.AreaLiving;
            section["Floors"] = realtyObject.Floors;
            section["MaximumFloors"] = realtyObject.MaximumFloors;
            section["NumberEntrances"] = realtyObject.NumberEntrances;
            section["NumberLiving"] = realtyObject.NumberLiving;
            section["PhysicalWear"] = realtyObject.PhysicalWear;
            section["RepairInadvisable"] = realtyObject.IsRepairInadvisable ? "Да" : "Нет";
            section["NumberApartments"] = realtyObject.NumberApartments;
            section["AreaLivingNotLivingMkd"] = realtyObject.AreaLivingNotLivingMkd;
            section["PrivatizationDate"] = realtyObject.PrivatizationDateFirstApartment.HasValue
                     ? realtyObject.PrivatizationDateFirstApartment.Value.ToString("dd.MM.yyyy")
                     : string.Empty;
            section["TypeProject"] = realtyObject.TypeProject;
            section["CadastreNumber"] = realtyObject.CadastreNumber;
            section["NecessaryConductCr"] = realtyObject.NecessaryConductCr.GetEnumMeta().Display;
            section["HasPrivatizedFlats"] = realtyObject.HasPrivatizedFlats.HasValue
                                                ? realtyObject.HasPrivatizedFlats.Value ? "Да" : "Нет"
                                                : string.Empty;

            section["QualityEstate"] = realtyObject.QualityEstate;
            section["CommonEstateObject"] = realtyObject.CommonEstateObject;
            section["seName"] = realtyObject.seName;

            if (realtyObject.isStructuralElement)
            {
                section["seVolume"] = realtyObject.seVolume;
                section["seLastOverhaulYear"] = realtyObject.seLastOverhaulYear;
                section["seWearout"] = realtyObject.seWearout;
            }
            else if (!realtyObject.isCommonEstateElement)
            {
                section["QualityApartments"] = realtyObject.QualityApartments;
                section["QualityAdditional"] = realtyObject.QualityAdditional;
                section["QualityCommon"] = realtyObject.QualityCommon;
                section["QualityMain"] = realtyObject.QualityMain;
            }
        }

        private void ProcessQuality(bool add, ref double value)
        {
            value = add ? ++value : value;
        }

        private class RoProxy
        {
            public string muName;
            public long Id;
            public string Address;
            public TypeHouse TypeHouse;
            public DateTime? DateCommissioning;
            public int? BuildYear;
            public decimal? AreaMkd;
            public decimal? AreaLiving;
            public int? Floors;
            public int? MaximumFloors;
            public int? NumberEntrances;
            public int? NumberLiving;
            public decimal? PhysicalWear;
            public DateTime? PrivatizationDateFirstApartment;
            public bool IsRepairInadvisable;
            public int? NumberApartments;
            public decimal? AreaLivingNotLivingMkd;
            public string MoSettlement;
            public bool? HasPrivatizedFlats;
            public YesNoNotSet NecessaryConductCr;
            public string TypeProject;
            public string CadastreNumber;
            public long MunicipalityId;
            public string MunicipalityName;
        }

        private class Row : RoProxy
        {
            public int num;
            public double QualityApartments;
            public double QualityEstate;
            public double QualityMain;
            public double QualityAdditional;
            public double QualityCommon;
            public string CommonEstateObject;
            public string seName;
            public int seLastOverhaulYear;
            public decimal seWearout;
            public decimal seVolume;
            public bool isStructuralElement;
            public bool isCommonEstateElement;

            public Row()
            {
            }

            public Row(RoProxy copy)
            {
                muName = copy.muName;
                Id = copy.Id;
                Address = copy.Address;
                TypeHouse = copy.TypeHouse;
                DateCommissioning = copy.DateCommissioning;
                BuildYear = copy.BuildYear;
                AreaMkd = copy.AreaMkd;
                AreaLiving = copy.AreaLiving;
                Floors = copy.Floors;
                MaximumFloors = copy.MaximumFloors;
                NumberEntrances = copy.NumberEntrances;
                NumberLiving = copy.NumberLiving;
                PhysicalWear = copy.PhysicalWear;
                PrivatizationDateFirstApartment = copy.PrivatizationDateFirstApartment;
                IsRepairInadvisable = copy.IsRepairInadvisable;
                NumberApartments = copy.NumberApartments;
                AreaLivingNotLivingMkd = copy.AreaLivingNotLivingMkd;
                MoSettlement = copy.MoSettlement;
                HasPrivatizedFlats = copy.HasPrivatizedFlats;
                NecessaryConductCr = copy.NecessaryConductCr;
                TypeProject = copy.TypeProject;
                CadastreNumber = copy.CadastreNumber;
                MunicipalityId = copy.MunicipalityId;
            }
        }
    }
}