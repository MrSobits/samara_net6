namespace Bars.Gkh.Overhaul.Reports
{
    using System.Linq;

    using Bars.B4;

    using B4.Modules.Reports;

    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Properties;

    using Castle.Windsor;

    /// <summary>
    /// Отчет для Экспорт технического паспорта
    /// </summary>
    public class RealtyObjectDataReport : BasePrintForm
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        private long roId;

        /// <summary>
        /// Конструктор
        /// </summary>
        public RealtyObjectDataReport()
            : base(new ReportTemplateBinary(Resources.RealtyObjectDataReport))
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "RealtyObjectPassport"; }
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        public override void SetUserParams(BaseParams baseParams)
        {
            this.roId = baseParams.Params.GetAs<long>("house");
        }

        /// <summary>
        /// Генератор отчета
        /// </summary>
        public override string ReportGenerator { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Наименование группы
        /// </summary>
        public override string GroupName
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Клиентский контроллер
        /// </summary>
        public override string ParamsController
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Ограничение прав доступа
        /// </summary>
        public override string RequiredPermission
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Подготовить отчет
        /// </summary>
        public override void PrepareReport(ReportParams reportParams)
        {
            var realityObjectService = this.Container.Resolve<IDomainService<RealityObject>>();
            var realityObjectStructuralElementService = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

            using (this.Container.Using(realityObjectService, realityObjectStructuralElementService))
            {
                var realityObjInfo = realityObjectService.GetAll()
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
                            x.HeatingSystem
                        })
                        .FirstOrDefault();

                if (realityObjInfo == null)
                {
                    return;
                }

                var realityObjectStructuralElements = realityObjectStructuralElementService
                        .GetAll()
                        .Where(x => x.RealityObject.Id == this.roId)
                        .Select(x => new
                        {
                            GroupName = x.StructuralElement.Group.Name,
                            x.StructuralElement.Name,
                            x.LastOverhaulYear,
                            x.Wearout,
                            x.Volume,
                            UnitMeasure = x.StructuralElement.UnitMeasure.Name,
                            State = x.State.Name
                        })
                        .ToList();

                reportParams.SimpleReportParams["Address"] = realityObjInfo.Address;
                reportParams.SimpleReportParams["TypeHouse"] = realityObjInfo.TypeHouse.GetEnumMeta().Display;
                reportParams.SimpleReportParams["ConditionHouse"] = realityObjInfo.ConditionHouse.GetEnumMeta().Display;
                reportParams.SimpleReportParams["BuildYear"] = realityObjInfo.BuildYear;
                reportParams.SimpleReportParams["DateCommissioning"] = realityObjInfo.DateCommissioning.HasValue ? realityObjInfo.DateCommissioning.Value.ToString("dd.MM.yyyy") : string.Empty;
                reportParams.SimpleReportParams["DateTechInspection"] = realityObjInfo.DateTechInspection.HasValue ? realityObjInfo.DateTechInspection.Value.ToString("dd.MM.yyyy") : string.Empty;
                reportParams.SimpleReportParams["PrivatizationDateFirstApartment"] = realityObjInfo.PrivatizationDateFirstApartment.HasValue ? realityObjInfo.PrivatizationDateFirstApartment.Value.ToString("dd.MM.yyyy") : string.Empty;
                reportParams.SimpleReportParams["FederalNum"] = realityObjInfo.FederalNum;
                reportParams.SimpleReportParams["Code"] = realityObjInfo.GkhCode;
                reportParams.SimpleReportParams["TypeOwnership"] = realityObjInfo.TypeOwnership;
                reportParams.SimpleReportParams["PhysicalWear"] = realityObjInfo.PhysicalWear;
                reportParams.SimpleReportParams["CadastreNumber"] = realityObjInfo.CadastreNumber;
                reportParams.SimpleReportParams["TotalBuildingVolume"] = realityObjInfo.TotalBuildingVolume;
                reportParams.SimpleReportParams["AreaMkd"] = realityObjInfo.AreaMkd;
                reportParams.SimpleReportParams["AreaOwned"] = realityObjInfo.AreaOwned;
                reportParams.SimpleReportParams["AreaMunicipalOwned"] = realityObjInfo.AreaMunicipalOwned;
                reportParams.SimpleReportParams["AreaGovernmentOwned"] = realityObjInfo.AreaGovernmentOwned;
                reportParams.SimpleReportParams["AreaLivingNotLivingMkd"] = realityObjInfo.AreaLivingNotLivingMkd;
                reportParams.SimpleReportParams["AreaLiving"] = realityObjInfo.AreaLiving;
                reportParams.SimpleReportParams["AreaLivingOwned"] = realityObjInfo.AreaLivingOwned;
                reportParams.SimpleReportParams["AreaNotLivingFunctional"] = realityObjInfo.AreaNotLivingFunctional;
                reportParams.SimpleReportParams["NecessaryConductCr"] = realityObjInfo.NecessaryConductCr.GetEnumMeta().Display;
                reportParams.SimpleReportParams["NumberApartments"] = realityObjInfo.NumberApartments;
                reportParams.SimpleReportParams["NumberLiving"] = realityObjInfo.NumberLiving;
                reportParams.SimpleReportParams["NumberLifts"] = realityObjInfo.NumberLifts;
                reportParams.SimpleReportParams["RoofingMaterial"] = realityObjInfo.RoofingMaterial;
                reportParams.SimpleReportParams["WallMaterial"] = realityObjInfo.WallMaterial;
                reportParams.SimpleReportParams["TypeRoof"] = realityObjInfo.TypeRoof.GetEnumMeta().Display;
                reportParams.SimpleReportParams["HeatingSystem"] = realityObjInfo.HeatingSystem.GetEnumMeta().Display;


                var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

                var num = 0;
                foreach (var structuralElement in realityObjectStructuralElements)
                {
                    ++num;

                    section.ДобавитьСтроку();

                    section["num"] = num;
                    section["grname"] = structuralElement.GroupName;
                    section["name"] = structuralElement.Name;
                    section["LastOvrhlYear"] = structuralElement.LastOverhaulYear;
                    section["Wearout"] = structuralElement.Wearout;
                    section["Volume"] = structuralElement.Volume;
                    section["UMeas"] = structuralElement.UnitMeasure;
                    section["State"] = structuralElement.State;
                } 
            }
        }
    }
}