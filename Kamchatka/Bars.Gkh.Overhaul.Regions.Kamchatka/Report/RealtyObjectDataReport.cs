namespace Bars.Gkh.Overhaul.Regions.Kamchatka.Report
{
    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Regions.Kamchatka.Properties;

    using Castle.Windsor;

    using System.Linq;

    public class RealtyObjectDataReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long roId;

        public RealtyObjectDataReport()
            : base(new ReportTemplateBinary(Resources.RealtyObjectDataReport))
        {
        }

        public override string Name
        {
            get { return "RealtyObjectDataReport"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            roId = baseParams.Params.GetAs<long>("house");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var realtyObjInfo = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => x.Id == this.roId)
                .Select(x => new
                {
                    Address = x.FiasAddress.AddressName,
                    x.TypeHouse,
                    x.ConditionHouse,
                    x.BuildYear,
                    x.DateCommissioning,
                    x.DateTechInspection,
                    x.PrivatizationDateFirstApartment,
                    x.FederalNum,
                    x.GkhCode,
                    TypeOwnership = x.TypeOwnership.Name,
                    x.PhysicalWear,
                    x.CadastreNumber,
                    x.TotalBuildingVolume,
                    x.AreaMkd,
                    x.AreaOwned,
                    x.AreaMunicipalOwned,
                    x.AreaGovernmentOwned,
                    x.AreaLivingNotLivingMkd,
                    x.AreaLiving,
                    x.AreaLivingOwned,
                    x.AreaNotLivingFunctional,
                    x.NecessaryConductCr,
                    x.NumberApartments,
                    x.NumberLiving,
                    x.NumberLifts,
                    RoofingMaterial = x.RoofingMaterial.Name,
                    WallMaterial = x.WallMaterial.Name,
                    x.TypeRoof,
                    x.HeatingSystem, 
                    CapitalGroup = x.CapitalGroup.Name,
                    x.MaximumFloors,
                    x.NumberEntrances
                })
                .FirstOrDefault();

            if (realtyObjInfo == null)
            {
                return;
            }

            var realityObjectStructuralElements =
                this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>()
                    .GetAll()
                    .Where(x => x.RealityObject.Id == this.roId)
                    .Select(x => new
                        {
                            GroupName = x.StructuralElement.Group.Name,
                            x.StructuralElement.Name,
                            x.LastOverhaulYear,
                            x.Wearout,
                            x.Volume,
                            UnitMeasure = x.StructuralElement.UnitMeasure.Name
                        })
                    .ToList();

            reportParams.SimpleReportParams["Address"] = realtyObjInfo.Address;
            reportParams.SimpleReportParams["TypeHouse"] = realtyObjInfo.TypeHouse.GetEnumMeta().Display;
            reportParams.SimpleReportParams["ConditionHouse"] = realtyObjInfo.ConditionHouse.GetEnumMeta().Display;
            reportParams.SimpleReportParams["BuildYear"] = realtyObjInfo.BuildYear;
            reportParams.SimpleReportParams["DateCommissioning"] = realtyObjInfo.DateCommissioning;
            reportParams.SimpleReportParams["DateTechInspection"] = realtyObjInfo.DateTechInspection;
            reportParams.SimpleReportParams["PrivatizationDateFirstApartment"] = realtyObjInfo.PrivatizationDateFirstApartment;
            reportParams.SimpleReportParams["FederalNum"] = realtyObjInfo.FederalNum;
            reportParams.SimpleReportParams["Code"] = realtyObjInfo.GkhCode;
            reportParams.SimpleReportParams["TypeOwnership"] = realtyObjInfo.TypeOwnership;
            reportParams.SimpleReportParams["PhysicalWear"] = realtyObjInfo.PhysicalWear;
            reportParams.SimpleReportParams["CadastreNumber"] = realtyObjInfo.CadastreNumber;
            reportParams.SimpleReportParams["TotalBuildingVolume"] = realtyObjInfo.TotalBuildingVolume;
            reportParams.SimpleReportParams["AreaMkd"] = realtyObjInfo.AreaMkd;
            reportParams.SimpleReportParams["AreaOwned"] = realtyObjInfo.AreaOwned;
            reportParams.SimpleReportParams["AreaMunicipalOwned"] = realtyObjInfo.AreaMunicipalOwned;
            reportParams.SimpleReportParams["AreaGovernmentOwned"] = realtyObjInfo.AreaGovernmentOwned;
            reportParams.SimpleReportParams["AreaLivingNotLivingMkd"] = realtyObjInfo.AreaLivingNotLivingMkd;
            reportParams.SimpleReportParams["AreaLiving"] = realtyObjInfo.AreaLiving;
            reportParams.SimpleReportParams["AreaLivingOwned"] = realtyObjInfo.AreaLivingOwned;
            reportParams.SimpleReportParams["AreaNotLivingFunctional"] = realtyObjInfo.AreaNotLivingFunctional;
            reportParams.SimpleReportParams["NecessaryConductCr"] = realtyObjInfo.NecessaryConductCr != 0 ? realtyObjInfo.NecessaryConductCr.GetEnumMeta().Display: string.Empty;
            reportParams.SimpleReportParams["NumberApartments"] = realtyObjInfo.NumberApartments;
            reportParams.SimpleReportParams["NumberLiving"] = realtyObjInfo.NumberLiving;
            reportParams.SimpleReportParams["NumberLifts"] = realtyObjInfo.NumberLifts;
            reportParams.SimpleReportParams["RoofingMaterial"] = realtyObjInfo.RoofingMaterial;
            reportParams.SimpleReportParams["WallMaterial"] = realtyObjInfo.WallMaterial;
            reportParams.SimpleReportParams["TypeRoof"] = realtyObjInfo.TypeRoof.GetEnumMeta().Display;
            reportParams.SimpleReportParams["HeatingSystem"] = realtyObjInfo.HeatingSystem.GetEnumMeta().Display;
            reportParams.SimpleReportParams["CapitalGroup"] = realtyObjInfo.CapitalGroup;
            reportParams.SimpleReportParams["MaxFloors"] = realtyObjInfo.MaximumFloors;
            reportParams.SimpleReportParams["NumberEntrances"] = realtyObjInfo.NumberEntrances;


            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var num = 0;
            foreach (var structuralElementGroup in realityObjectStructuralElements.GroupBy(x => x.GroupName))
            {
                ++num;

                foreach (var structuralElement in structuralElementGroup)
                {
                    section.ДобавитьСтроку();

                    section["num"] = num;
                    section["grname"] = structuralElement.GroupName;
                    section["name"] = structuralElement.Name;
                    section["LastOvrhlYear"] = structuralElement.LastOverhaulYear;
                    section["Wearout"] = decimal.ToInt64(structuralElement.Wearout);
                    section["Volume"] = structuralElement.Volume;
                    section["UMeas"] = structuralElement.UnitMeasure;
                }
            }
        }

        public override string Desciption
        {
            get
            {
                return "Информация по дому";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Жилые дома";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.RealityObjectDataReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "";
            }
        }
    }
}