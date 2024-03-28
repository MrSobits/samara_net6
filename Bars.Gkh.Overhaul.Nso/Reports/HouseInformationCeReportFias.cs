namespace Bars.Gkh.Overhaul.Nso.Reports
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FIAS;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Properties;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;

    public class HouseInformationCeReportFias : BasePrintForm
    {
        public HouseInformationCeReportFias()
            : base(new ReportTemplateBinary(Resources.HouseInformationFias))
        {
        }

        private long[] municipalityIds;

        private int[] houseTypes;

        private int housesList;

        public IWindsorContainer Container { get; set; }
        public IDomainService<ManOrgContractRealityObject> ManOrgBaseContractService { get; set; }
        public IDomainService<RealityObjectMissingCeo> RealityObjectMissingCeoService { get; set; }
        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }
        public IDomainService<Fias> FiasDomain { get; set; } 
        
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

            housesList = baseParams.Params.GetAs<int>("housesList");
        }

        public override string Name
        {
            get { return "Информация по домам (ФИАС)"; }
        }

        public override string Desciption
        {
            get { return "Информация по домам (ФИАС)"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.HouseInformationCeFiasReport";
            }
        }
        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.HouseInformationCeFiasReport";
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            long[] realObjectIds = null;

            if (housesList == 2)
            {
                realObjectIds =
                    PublishedProgramRecordDomain.GetAll()
                        .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                        .Select(x => x.Stage2.Stage3Version.RealityObject.Id)
                        .Distinct()
                        .ToArray();
            }

            var realtyObjects = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.TypeHouse))
                .WhereIf(realObjectIds != null, x => realObjectIds.Contains(x.Id))
                .Select(x => new
                {
                    Municipality = x.Municipality.Name,
                    Settlement = x.MoSettlement != null ? x.MoSettlement.Name : string.Empty,
                    x.Address,
                    x.Id,
                    x.ExternalId,
                    x.FiasAddress.PlaceName,
                    x.FiasAddress.StreetName,
                    x.FiasAddress.StreetGuidId,
                    x.FiasAddress.House,
                    x.FiasAddress.Housing,
                    x.FiasAddress.Letter,
                    x.FiasAddress.Building,
                    ConditionHouse = x.ConditionHouse.GetEnumMeta().Display,
                    TypeHouse = x.TypeHouse.GetEnumMeta().Display,
                    x.DateCommissioning,
                    x.BuildYear,
                    x.AreaMkd,
                    x.AreaOwned,
                    x.AreaLivingNotLivingMkd,
                    x.AreaLivingOwned,
                    x.AreaLiving,
                    x.AreaMunicipalOwned,
                    x.AreaNotLivingFunctional,
                    x.AreaGovernmentOwned,
                    x.AreaCommonUsage,
                    x.Floors,
                    x.MaximumFloors,
                    x.NumberEntrances,
                    x.NumberApartments,
                    x.NumberLiving,
                    x.PhysicalWear,
                    x.PrivatizationDateFirstApartment,
                    x.IsRepairInadvisable
                })
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Settlement)
                .ToList();

            var manorgRoDict = ManOrgBaseContractService.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                 .GroupBy(x => x.RealityObject.Id)
                 .ToDictionary(x => x.Key, arg => new
                 {
                     ManOrg = arg.Select(z => z.ManOrgContract).OrderByDescending(z => z.StartDate).FirstOrDefault()
                 });


            var missingCeoDict = RealityObjectMissingCeoService.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                .GroupBy(x => x.RealityObject.Id)
                 .ToDictionary(x => x.Key, arg => new
                 {
                     MissingCeoList = arg.Select(z => z.MissingCommonEstateObject.Id).ToList()
                 });

            var realityObjectStructuralElementsDict = this.Container
                .Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .Where(x => x.State.StartState)
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
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

            var structuralElementsDict = this.Container
               .Resolve<IDomainService<StructuralElement>>().GetAll()
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

            var streetKladrCodeByAoGuid = FiasDomain.GetAll()
               .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
               .Where(x => x.AOLevel == FiasLevelEnum.Street)
               .Where(x => x.ParentGuid != null && x.ParentGuid != string.Empty)
               .Select(x => new
               {
                   x.AOGuid,
                   x.CodeStreet
               })
               .AsEnumerable()
               .GroupBy(x => x.AOGuid)
               .ToDictionary(x => x.Key, x => x.OrderBy(y => y.CodeStreet).First().CodeStreet);


            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("vertSection");

            foreach (var group in structuralElementsDict.Keys)
            {
                foreach (var elem in structuralElementsDict[group])
                {
                    verticalSection.ДобавитьСтроку();
                    verticalSection["groupname"] = group.Name;
                    verticalSection["elemName"] = elem.Name;

                    verticalSection["year"] = string.Format("$year{0}$", elem.Id);
                    verticalSection["wear"] = string.Format("$wear{0}$", elem.Id);
                    verticalSection["volume"] = string.Format("$volume{0}$", elem.Id);
                }
            }

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var num = 0;

            foreach (var realtyObject in realtyObjects)
            {
                section.ДобавитьСтроку();

                section["Num"] = ++num;
                section["ExternalId"] = realtyObject.ExternalId;
                section["Municipality"] = realtyObject.Municipality;
                section["Settlement"] = realtyObject.Settlement;
                section["Address"] = realtyObject.Address;
                char[] charsToTrim = { '.' };
                if (!realtyObject.PlaceName.IsEmpty())
                {
                    var placeNameIndex = realtyObject.PlaceName.IndexOf(" ", StringComparison.Ordinal);

                    if (placeNameIndex != -1)
                    {
                        section["PlaceNameType"] = realtyObject.PlaceName.Substring(0, placeNameIndex).Trim(charsToTrim);
                        section["PlaceName"] = realtyObject.PlaceName.Substring(placeNameIndex + 1, realtyObject.PlaceName.Length - 1 - placeNameIndex);
                    }
                    else
                    {
                        section["PlaceNameType"] = string.Empty;
                        section["PlaceName"] = realtyObject.PlaceName;
                    }
                }
                if (!realtyObject.StreetName.IsEmpty())
                {
                    var streetIndex = realtyObject.StreetName.IndexOf(" ", StringComparison.Ordinal);
                    if (streetIndex != -1)
                    {
                        section["StreetType"] = realtyObject.StreetName.Substring(0, streetIndex).Trim(charsToTrim);
                        section["Street"] = realtyObject.StreetName.Substring(streetIndex + 1, realtyObject.StreetName.Length - 1 - streetIndex);
                    }
                    else
                    {
                        section["StreetType"] = string.Empty;
                        section["Street"] = realtyObject.StreetName;
                    }
                }
                section["StreetCode"] = realtyObject.StreetGuidId != null
                    ? streetKladrCodeByAoGuid.ContainsKey(realtyObject.StreetGuidId)
                        ? streetKladrCodeByAoGuid[realtyObject.StreetGuidId]
                        : string.Empty
                    : string.Empty;
                section["House"] = realtyObject.House;
                section["Housing"] = realtyObject.Housing;
                section["Letter"] = realtyObject.Letter;
                section["Building"] = realtyObject.Building;
                section["ConditionHouse"] = realtyObject.ConditionHouse;
                section["TypeHouse"] = realtyObject.TypeHouse;
                section["DateCommissioning"] = realtyObject.DateCommissioning.HasValue
                    ? realtyObject.DateCommissioning.Value.ToString("dd.MM.yyyy")
                    : string.Empty;
                section["BuildYear"] = realtyObject.BuildYear;
                section["AreaMkd"] = realtyObject.AreaMkd;
                section["AreaOwned"] = realtyObject.AreaOwned;
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
                section["NumberApartments"] = realtyObject.NumberApartments;
                section["PhysicalWear"] = realtyObject.PhysicalWear;
                section["IsRepairInadvisable"] = realtyObject.IsRepairInadvisable ? "да" : "нет";

                section["ManOrgName"] = manorgRoDict.ContainsKey(realtyObject.Id)
                    ? manorgRoDict[realtyObject.Id].ManOrg.ReturnSafe(x => x.ManagingOrganization)
                        .ReturnSafe(x => x.Contragent)
                        .ReturnSafe(x => x.Name)
                    : "";

                section["PrivatizationDate"] = realtyObject.PrivatizationDateFirstApartment.HasValue
                    ? realtyObject.PrivatizationDateFirstApartment.Value.ToString("dd.MM.yyyy")
                    : string.Empty;


                foreach (var ceo in structuralElementsDict.Keys)
                {
                    foreach (var elem in structuralElementsDict[ceo])
                    {
                        if (realityObjectStructuralElementsDict.ContainsKey(realtyObject.Id))
                        {
                            if (missingCeoDict.ContainsKey(realtyObject.Id) && (missingCeoDict[realtyObject.Id].MissingCeoList.Contains(elem.CommonEstateObject.Id)))
                            {
                                section[string.Format("volume{0}", elem.Id)] = "-";
                                section[string.Format("year{0}", elem.Id)] = "-";
                                section[string.Format("wear{0}", elem.Id)] = "-";
                            }
                            else if (realityObjectStructuralElementsDict[realtyObject.Id].Any(x => x.elemId == elem.Id))
                            {
                                var realityObjectStructuralElement =
                                    realityObjectStructuralElementsDict[realtyObject.Id].FirstOrDefault(x => x.elemId == elem.Id);

                                section[string.Format("volume{0}", elem.Id)] = realityObjectStructuralElement.Volume;
                                section[string.Format("year{0}", elem.Id)] = realityObjectStructuralElement.LastOverhaulYear;
                                section[string.Format("wear{0}", elem.Id)] = realityObjectStructuralElement.Wearout;
                            }
                            else if (realityObjectStructuralElementsDict[realtyObject.Id].All(x => x.GroupName != elem.GroupName))
                            {
                                section[string.Format("volume{0}", elem.Id)] = "#";
                            }
                        }
                    }
                }
            }
        }
    }
}
