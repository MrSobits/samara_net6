namespace Bars.Gkh.Overhaul.Nso.Reports
{
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Nso.Properties;

    using Castle.Windsor;
    using Overhaul.Entities;

    class CertificationControlValues : BasePrintForm
    {
        public CertificationControlValues()
            : base(new ReportTemplateBinary(Resources.CertificationControlValues))
        {
        }

        private long[] municipalityIds;

        private int[] houseTypes;

        public IWindsorContainer Container { get; set; }

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
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var realtyObjects = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.TypeHouse))
                .Select(x => new
                    {
                        muName = x.Municipality.Name,
                        x.Id,
                        x.Address,
                        TypeHouse = x.TypeHouse.GetEnumMeta().Display,
                        x.DateCommissioning,
                        x.BuildYear,
                        x.AreaMkd,
                        x.AreaOwned,
                        x.AreaLiving,
                        x.AreaLivingOwned,
                        x.AreaMunicipalOwned,
                        x.AreaNotLivingFunctional,
                        x.AreaGovernmentOwned,
                        x.AreaCommonUsage,
                        x.Floors,
                        x.MaximumFloors,
                        x.NumberEntrances,
                        x.NumberLiving,
                        x.PhysicalWear,
                        x.PrivatizationDateFirstApartment,
                        x.IsRepairInadvisable
                    })
                .OrderBy(x => x.muName)
                .ThenBy(x => x.Address)
                .ToList();

            var realityObjectStructuralElementsDict = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                .Where(x => x.StructuralElement.Group.CommonEstateObject.IncludedInSubjectProgramm)
                .Where(x => x.StructuralElement.Group.UseInCalc)
                .Where(x => x.State.StartState)
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    Name = x.Name ?? x.StructuralElement.Name,
                    x.Volume,
                    x.LastOverhaulYear,
                    x.Wearout
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var num = 0;

            foreach (var realtyObject in realtyObjects)
            {
                section.ДобавитьСтроку();

                section["Num"] = ++num;
                section["MuName"] = realtyObject.muName;
                section["Address"] = realtyObject.Address;
                section["TypeHouse"] = realtyObject.TypeHouse;
                section["DateCommissioning"] = realtyObject.DateCommissioning.HasValue ? realtyObject.DateCommissioning.Value.ToString("dd.MM.yyyy") : string.Empty;
                section["BuildYear"] = realtyObject.BuildYear;
                section["AreaMkd"] = realtyObject.AreaMkd;
                section["AreaOwned"] = realtyObject.AreaOwned;
                section["AreaMunicipalOwned"] = realtyObject.AreaMunicipalOwned;
                section["AreaGovernmentOwned"] = realtyObject.AreaGovernmentOwned;
                section["AreaLiving"] = realtyObject.AreaLiving;
                section["AreaLivingOwned"] = realtyObject.AreaLivingOwned;
                section["AreaNotLivingFunctional"] = realtyObject.AreaNotLivingFunctional;
                section["AreaCommonUsage"] = realtyObject.AreaCommonUsage;
                section["Floors"] = realtyObject.Floors;
                section["MaximumFloors"] = realtyObject.MaximumFloors;
                section["NumberEntrances"] = realtyObject.NumberEntrances;
                section["NumberLiving"] = realtyObject.NumberLiving;
                section["PhysicalWear"] = realtyObject.PhysicalWear;
                section["IsRepairInadvisable"] = realtyObject.IsRepairInadvisable ? "да" : "нет";

                section["PrivatizationDate"] = realtyObject.PrivatizationDateFirstApartment.HasValue
                    ? realtyObject.PrivatizationDateFirstApartment.Value.ToString("dd.MM.yyyy")
                    : string.Empty;
                if (!realityObjectStructuralElementsDict.ContainsKey(realtyObject.Id))
                {
                    continue;
                }
                
                var firstLine = true;
                foreach (var realityObjectStructuralElement in realityObjectStructuralElementsDict[realtyObject.Id])
                {
                    if (firstLine)
                    {
                        firstLine = false;
                    }
                    else
                    {
                        section.ДобавитьСтроку();
                    }

                    section["seName"] = realityObjectStructuralElement.Name;
                    section["seVolume"] = realityObjectStructuralElement.Volume;
                    section["seLastOverhaulYear"] = realityObjectStructuralElement.LastOverhaulYear;
                    section["seWearout"] = realityObjectStructuralElement.Wearout;
                }
            }
        }
    }
}


