namespace Bars.GkhCr.Report
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
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class FillingPassportControl : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long[] municipalityIds;
        private long programCrId;

        public FillingPassportControl()
            : base(new ReportTemplateBinary(Properties.Resources.FillingPassportControl))
        {
        }

        public override string Name
        {
            get
            {
                return "Контроль заполнения паспорта";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Контроль заполнения паспорта";
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
                return "B4.controller.report.FillingPassportControl";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.FillingPassportControl";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();

            var m = baseParams.Params["municipalityIds"].ToStr();
            this.municipalityIds = !string.IsNullOrEmpty(m) ? m.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceProgramCr = this.Container.Resolve<IDomainService<ProgramCr>>();
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();
            var serviceObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>();
            var serviceManOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var serviceContragentContact = this.Container.Resolve<IDomainService<ContragentContact>>();

            var municipalityDict = serviceMunicipality.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, x => x.Name);

            var programCrDates = serviceProgramCr.GetAll()
                .Where(x => x.Id == this.programCrId)
                .Select(x => new { x.Period.DateStart, x.Period.DateEnd })
                .FirstOrDefault();

            if (programCrDates == null)
            {
                throw new Exception("Некорректный период программы капремонта");
            }

            var programmYear = programCrDates.DateStart.Year;
            var programCrStartDate = programCrDates.DateStart;
            var programCrEndDate = programCrDates.DateEnd ?? programCrDates.DateStart.AddYears(1).AddDays(-1);

            var objectCrQuery = serviceObjectCr.GetAll()
                .Where(x => x.ProgramCr.Id == this.programCrId)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id));

            var roIdsQuery = objectCrQuery.Select(x => x.RealityObject.Id);

            var managingOrgByRoId = serviceManOrgContractRealityObject.GetAll()
                .Where(x => roIdsQuery.Contains(x.RealityObject.Id))
                .Where(x => x.ManOrgContract.StartDate <= programCrEndDate)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= programCrStartDate)
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Select(x => new
                {
                    x.ManOrgContract.TypeContractManOrgRealObj,
                    RoId = x.RealityObject.Id,
                    x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    x.ManOrgContract.ManagingOrganization.TypeManagement,
                    ContragentId = (long?)x.ManOrgContract.ManagingOrganization.Contragent.Id,
                    x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                    x.ManOrgContract.ManagingOrganization.Contragent.Kpp,
                    x.ManOrgContract.ManagingOrganization.Contragent.Phone,
                    x.ManOrgContract.ManagingOrganization.Contragent.FiasFactAddress.PostCode,
                    x.ManOrgContract.ManagingOrganization.Contragent.FiasFactAddress.House,
                    x.ManOrgContract.ManagingOrganization.Contragent.FiasFactAddress.StreetName,
                    x.ManOrgContract.ManagingOrganization.Contragent.FiasFactAddress.PlaceName
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.TypeContractManOrgRealObj).First());

            var contragentIdsQuery = serviceManOrgContractRealityObject.GetAll()
                .Where(x => roIdsQuery.Contains(x.RealityObject.Id))
                .Where(x => x.ManOrgContract.StartDate <= programCrEndDate)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= programCrStartDate)
                .Where(x => x.ManOrgContract.ManagingOrganization.Contragent != null)
                .Select(x => x.ManOrgContract.ManagingOrganization.Contragent.Id);

            var contragentContactsFioDict = serviceContragentContact.GetAll()
                .Where(x => x.Position.Code == "1")
                .Where(x => contragentIdsQuery.Contains(x.Contragent.Id))
                .Select(x => new { x.FullName, x.Contragent.Id })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.First().FullName);

            var objectCrByMuDict = objectCrQuery
                .Select(x => new
                {
                    x.RealityObject.Address,
                    RoId = x.RealityObject.Id,
                    MunicipalityId = x.RealityObject.Municipality.Id,
                    CapitalGroup = x.RealityObject.CapitalGroup.Name,
                    x.ProgramNum,
                    x.RealityObject.MaximumFloors,
                    x.RealityObject.PhysicalWear,
                    x.RealityObject.AreaMkd,
                    x.RealityObject.AreaLivingNotLivingMkd,
                    x.RealityObject.AreaLiving,
                    x.RealityObject.AreaLivingOwned,
                    x.RealityObject.NumberApartments,
                    x.RealityObject.NumberLiving,
                    WallMaterialName = x.RealityObject.WallMaterial.Name,
                    RoofingMaterialName = x.RealityObject.RoofingMaterial.Name,
                    DateCommissioning = x.RealityObject.DateCommissioning.HasValue ? x.RealityObject.DateCommissioning.Value.Year.ToStr() : string.Empty,
                    DateLastOverhaul = x.RealityObject.DateLastOverhaul.HasValue ? x.RealityObject.DateLastOverhaul.Value.Year.ToStr() : string.Empty
                })
                .AsEnumerable()
                .Select(x =>
                {
                    var manorgData = managingOrgByRoId.ContainsKey(x.RoId) ? managingOrgByRoId[x.RoId] : null;
                    var contragentId = manorgData.Return(y => y.ContragentId);
                    var contragentContactsFio = contragentId.HasValue && contragentContactsFioDict.ContainsKey(contragentId.Value)
                                                    ? contragentContactsFioDict[contragentId.Value]
                                                    : string.Empty;

                    return new
                    {
                        x.Address,
                        x.RoId,
                        x.MunicipalityId,
                        x.CapitalGroup,
                        x.ProgramNum,
                        x.MaximumFloors,
                        x.PhysicalWear,
                        x.AreaMkd,
                        x.AreaLivingNotLivingMkd,
                        x.AreaLiving,
                        x.AreaLivingOwned,
                        x.NumberApartments,
                        x.NumberLiving,
                        x.WallMaterialName,
                        x.RoofingMaterialName,
                        x.DateCommissioning,
                        x.DateLastOverhaul,

                        Name = manorgData.Return(y => y.Name),
                        TypeManagement = manorgData.Return(y => y.TypeManagement),
                        Inn = manorgData.Return(y => y.Inn),
                        Kpp = manorgData.Return(y => y.Kpp),
                        Phone = manorgData.Return(y => y.Phone),
                        PostCode = manorgData.Return(y => y.PostCode),
                        House = manorgData.Return(y => y.House),
                        StreetName = manorgData.Return(y => y.StreetName),
                        PlaceName = manorgData.Return(y => y.PlaceName),
                        contragentContactsFio
                    };
                })
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.RoId).Select(y => y.First()).ToList());

            var warningsCountByMuId = objectCrByMuDict
                .ToDictionary(
                x => x.Key, 
                y => new
                    {
                        Floors = y.Value.Count(z => !z.MaximumFloors.HasValue),
                        CapitalGroup = y.Value.Count(z => z.CapitalGroup.IsEmpty()),
                        AreaMkd = y.Value.Count(z => !z.AreaMkd.HasValue),
                        AreaLivingNotLivingMkd = y.Value.Count(z => !z.AreaLivingNotLivingMkd.HasValue),
                        AreaLiving = y.Value.Count(z => !z.AreaLiving.HasValue),
                        AreaLivingOwned = y.Value.Count(z => !z.AreaLivingOwned.HasValue),
                        NumberApartments = y.Value.Count(z => !z.NumberApartments.HasValue),
                        NumberLiving = y.Value.Count(z => !z.NumberLiving.HasValue),
                        WallMaterial = y.Value.Count(z => z.WallMaterialName.IsEmpty()),
                        RoofingMaterial = y.Value.Count(z => z.RoofingMaterialName.IsEmpty()),
                        DateCommissioning = y.Value.Count(z => z.DateCommissioning.IsEmpty()),
                        DateLastOverhaul = y.Value.Count(z => z.DateLastOverhaul.IsEmpty()),
                        PhysicalWear = y.Value.Count(z => !z.PhysicalWear.HasValue),

                        typeControlUkCount = y.Value.Count(z => z.TypeManagement == TypeManagementManOrg.UK),
                        Name = y.Value.Count(z => z.Name.IsEmpty()),
                        Inn = y.Value.Count(z => z.Inn.IsEmpty()),
                        Kpp = y.Value.Count(z => z.Kpp.IsEmpty()),
                        Phone = y.Value.Count(z => z.Phone.IsEmpty()),
                        PostCode = y.Value.Count(z => z.PostCode.IsEmpty()),
                        House = y.Value.Count(z => z.House.IsEmpty()),
                        StreetName = y.Value.Count(z => z.StreetName.IsEmpty()),
                        PlaceName = y.Value.Count(z => z.PlaceName.IsEmpty()),
                        BassFam = y.Value.Count(z => z.contragentContactsFio.IsEmpty())
                    });

            var TotalDict = new Dictionary<string, decimal>()
            {
                { "IISMKDControlType", 0 },
                { "IISOrganizationName", 0 },
                { "IISSettlement", 0 },
                { "IISIndexUO", 0 },
                { "IISStreetUO", 0 },
                { "IISHouseUO", 0 },
                { "IISINN", 0 },
                { "IISKPP", 0 },
                { "IISBossFam", 0 },
                { "IISPhoneOrg", 0 },
                { "IISFlors", 0 },
                { "IISCapitalGroup", 0 },
                { "IISTotalAreaMKD", 0 },
                { "IISTotalAreaRoomsMKD", 0 },
                { "IISTotalArea", 0 },
                { "IISPropertyArea", 0 },
                { "IISNumberApartments", 0 },
                { "IISLivingCount", 0 },
                { "IISWallMaterial", 0 },
                { "IISRoofMaterial", 0 },
                { "IISOperationYear", 0 },
                { "IISWearProcent", 0 },
                { "IISYearLastRepair", 0 },
                { "ITotalAreaMKD", 0 },
                { "ITotalAreaRoomsMKD", 0 },
                { "ITotalArea", 0 },
                { "IPropertyArea", 0 },
                { "INumberApartments", 0 },
                { "ILivingCount", 0 },
                { "IRDict", 0 }
            };

            var IRDict = new Dictionary<string, decimal>()
            {
                { "IRTotalAreaMKD", 0 },
                { "IRTotalAreaRoomsMKD", 0 },
                { "IRTotalArea", 0 },
                { "IRPropertyArea", 0 },
                { "IRNumberApartments", 0 },
                { "IRLivingCount", 0 }
            };

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("НачалоСекции");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");

            reportParams.SimpleReportParams["ProgrammYear"] = programmYear;

            foreach (var municipality in municipalityDict.OrderBy(x => x.Value))
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["MoName"] = municipality.Value;

                if (!objectCrByMuDict.ContainsKey(municipality.Key))
                {
                    continue;
                }

                var warningsCount = warningsCountByMuId[municipality.Key];

                IRDict.Keys.ToList().ForEach(x => IRDict[x] = 0);

                int count = 0;

                foreach (var objectCr in objectCrByMuDict[municipality.Key].OrderBy(x => x.Address))
                {
                    sectionRo.ДобавитьСтроку();

                    sectionRo["Number"] = ++count;
                    sectionRo["ProgrammNumber"] = objectCr.ProgramNum;
                    sectionRo["RoAdress"] = objectCr.Address;
                    sectionRo["MKDControlType"] = objectCr.TypeManagement.GetEnumMeta().Display;
                    sectionRo["OrganizationName"] = objectCr.Name;
                    sectionRo["Settlement"] = objectCr.PlaceName;
                    sectionRo["IndexUO"] = objectCr.PostCode;
                    sectionRo["StreetUO"] = objectCr.StreetName;
                    sectionRo["HouseUO"] = objectCr.House;
                    sectionRo["INN"] = objectCr.Inn;
                    sectionRo["KPP"] = objectCr.Kpp;
                    sectionRo["BossFam"] = objectCr.contragentContactsFio;
                    sectionRo["PhoneOrg"] = objectCr.Phone;
                    sectionRo["этажность"] = objectCr.MaximumFloors;
                    sectionRo["CapitalGroup"] = objectCr.CapitalGroup;
                    sectionRo["TotalAreaMKD"] = objectCr.AreaMkd;
                    sectionRo["TotalAreaRoomsMKD"] = objectCr.AreaLivingNotLivingMkd;
                    sectionRo["TotalArea"] = objectCr.AreaLiving;
                    sectionRo["PropertyArea"] = objectCr.AreaLivingOwned;
                    sectionRo["NumberApartments"] = objectCr.NumberApartments;
                    sectionRo["LivingCount"] = objectCr.NumberLiving;
                    sectionRo["WallMaterial"] = objectCr.WallMaterialName;
                    sectionRo["RoofMaterial"] = objectCr.RoofingMaterialName;
                    sectionRo["OperationYear"] = objectCr.DateCommissioning;
                    sectionRo["WearProcent"] = objectCr.PhysicalWear;
                    sectionRo["YearLastRepair"] = objectCr.DateLastOverhaul;

                    IRDict["IRTotalAreaMKD"] += objectCr.AreaMkd.ToDecimal();
                    IRDict["IRTotalAreaRoomsMKD"] += objectCr.AreaLivingNotLivingMkd.ToDecimal();
                    IRDict["IRTotalArea"] += objectCr.AreaLiving.ToDecimal();
                    IRDict["IRPropertyArea"] += objectCr.AreaLivingOwned.ToDecimal();
                    IRDict["IRNumberApartments"] += objectCr.NumberApartments.ToDecimal();
                    IRDict["IRLivingCount"] += objectCr.NumberLiving.ToDecimal();
                }

                TotalDict["ITotalAreaMKD"] += IRDict["IRTotalAreaMKD"];
                TotalDict["ITotalAreaRoomsMKD"] += IRDict["IRTotalAreaRoomsMKD"];
                TotalDict["ITotalArea"] += IRDict["IRTotalArea"];
                TotalDict["IPropertyArea"] += IRDict["IRPropertyArea"];
                TotalDict["INumberApartments"] += IRDict["IRNumberApartments"];
                TotalDict["ILivingCount"] += IRDict["IRLivingCount"];

                sectionMu["ISMKDControlType"] = warningsCount.typeControlUkCount;
                sectionMu["ISOrganizationName"] = warningsCount.Name;
                sectionMu["ISSettlement"] = warningsCount.PlaceName;
                sectionMu["ISIndexUO"] = warningsCount.PostCode;
                sectionMu["ISStreetUO"] = warningsCount.StreetName;
                sectionMu["ISHouseUO"] = warningsCount.House;
                sectionMu["ISINN"] = warningsCount.Inn;
                sectionMu["ISKPP"] = warningsCount.Kpp;
                sectionMu["ISBossFam"] = warningsCount.BassFam;
                sectionMu["ISPhoneOrg"] = warningsCount.Phone;
                sectionMu["ISFlors"] = warningsCount.Floors;
                sectionMu["ISCapitalGroup"] = warningsCount.CapitalGroup;
                sectionMu["ISTotalAreaMKD"] = warningsCount.AreaMkd;
                sectionMu["ISTotalAreaRoomsMKD"] = warningsCount.AreaLivingNotLivingMkd;
                sectionMu["ISTotalArea"] = warningsCount.AreaLiving;
                sectionMu["ISPropertyArea"] = warningsCount.AreaLivingOwned;
                sectionMu["ISNumberApartments"] = warningsCount.NumberApartments;
                sectionMu["ISLivingCount"] = warningsCount.NumberLiving;
                sectionMu["ISWallMaterial"] = warningsCount.WallMaterial;
                sectionMu["ISRoofMaterial"] = warningsCount.RoofingMaterial;
                sectionMu["ISOperationYear"] = warningsCount.DateCommissioning;
                sectionMu["ISWearProcent"] = warningsCount.PhysicalWear;
                sectionMu["ISYearLastRepair"] = warningsCount.DateLastOverhaul;
                
                TotalDict["IISMKDControlType"] += warningsCount.typeControlUkCount;
                TotalDict["IISOrganizationName"] += warningsCount.Name;
                TotalDict["IISSettlement"] += warningsCount.PlaceName;
                TotalDict["IISIndexUO"] += warningsCount.PostCode;
                TotalDict["IISStreetUO"] += warningsCount.StreetName;
                TotalDict["IISHouseUO"] += warningsCount.House;
                TotalDict["IISINN"] += warningsCount.Inn;
                TotalDict["IISKPP"] += warningsCount.Kpp;
                TotalDict["IISBossFam"] += warningsCount.BassFam;
                TotalDict["IISPhoneOrg"] += warningsCount.Phone;
                TotalDict["IISFlors"] += warningsCount.Floors;
                TotalDict["IISCapitalGroup"] += warningsCount.CapitalGroup;
                TotalDict["IISTotalAreaMKD"] += warningsCount.AreaMkd;
                TotalDict["IISTotalAreaRoomsMKD"] += warningsCount.AreaLivingNotLivingMkd;
                TotalDict["IISTotalArea"] += warningsCount.AreaLiving;
                TotalDict["IISPropertyArea"] += warningsCount.AreaLivingOwned;
                TotalDict["IISNumberApartments"] += warningsCount.NumberApartments;
                TotalDict["IISLivingCount"] += warningsCount.NumberLiving;
                TotalDict["IISWallMaterial"] += warningsCount.WallMaterial;
                TotalDict["IISRoofMaterial"] += warningsCount.RoofingMaterial;
                TotalDict["IISOperationYear"] += warningsCount.DateCommissioning;
                TotalDict["IISWearProcent"] += warningsCount.PhysicalWear;
                TotalDict["IISYearLastRepair"] += warningsCount.DateLastOverhaul;

                foreach (var ir in IRDict)
                {
                    sectionMu[ir.Key] = ir.Value;
                }
            }

            foreach (var total in TotalDict)
            {
                reportParams.SimpleReportParams[total.Key] = total.Value;
            }
        }
    }
}