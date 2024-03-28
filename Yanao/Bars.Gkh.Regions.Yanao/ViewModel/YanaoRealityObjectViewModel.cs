namespace Bars.Gkh.Regions.Yanao.ViewModel
{
    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.Regions.Yanao.Entities;
    using Bars.Gkh.ViewModel;

    using Gkh.Entities;

    using System.Linq;
    using Utils;

    public class YanaoRealityObjectViewModel : RealityObjectViewModel
    {
        public IDomainService<RealityObjectExtension> RealityObjectExtensionService { get; set; }

        public override IDataResult Get(IDomainService<RealityObject> domainService, BaseParams baseParams)
        {
            var realityObject = domainService.Get(ObjectParseExtention.To<long>(baseParams.Params["id"]));

            if (realityObject == null)
            {
                return new BaseDataResult();
            }

            var realityObjectExtension = RealityObjectExtensionService.GetAll().FirstOrDefault(x => x.RealityObject.Id == realityObject.Id);

            var result = new
                    {
                        realityObject.Id,
                        realityObject.Municipality,
                        FiasAddress = realityObject.FiasAddress.GetFiasProxy(this.Container),
                        realityObject.Address,
                        realityObject.DeleteAddressId,
                        realityObject.Description,
                        realityObject.CodeErc,
                        realityObject.AreaLiving,
                        realityObject.AreaLivingOwned,
                        realityObject.AreaLivingNotLivingMkd,
                        realityObject.AreaMkd,
                        realityObject.AreaBasement,
                        realityObject.DateLastOverhaul,
                        realityObject.DateCommissioning,
                        realityObject.CapitalGroup,
                        realityObject.DateDemolition,
                        realityObject.MaximumFloors,
                        realityObject.RoofingMaterial,
                        realityObject.WallMaterial,
                        realityObject.IsInsuredObject,
                        realityObject.Notation,
                        realityObject.SeriesHome,
                        realityObject.PhysicalWear,
                        realityObject.TypeOwnership,
                        realityObject.Floors,
                        realityObject.FederalNum,
                        realityObject.NumberApartments,
                        realityObject.NumberEntrances,
                        realityObject.NumberLifts,
                        realityObject.NumberLiving,
                        realityObject.HavingBasement,
                        realityObject.HeatingSystem,
                        realityObject.ConditionHouse,
                        realityObject.TypeHouse,
                        realityObject.TypeRoof,
                        realityObject.WebCameraUrl,
                        realityObject.DateTechInspection,
                        realityObject.ResidentsEvicted,
                        realityObject.TypeProject,
                        realityObject.GkhCode,
                        realityObject.IsBuildSocialMortgage,
                        realityObject.AreaOwned,
                        realityObject.AreaMunicipalOwned,
                        realityObject.AreaGovernmentOwned,
                        realityObject.AreaNotLivingFunctional,
                        realityObject.TotalBuildingVolume,
                        realityObject.CadastreNumber,
                        realityObject.FloorHeight,
                        realityObject.PrivatizationDateFirstApartment,
                        realityObject.BuildYear,
                        realityObject.State,
                        realityObject.PercentDebt,
                        realityObject.NecessaryConductCr,
                        realityObject.MethodFormFundCr,
                        realityObject.HasJudgmentCommonProp,
                        realityObject.IsRepairInadvisable,
                        realityObject.HasPrivatizedFlats,
                        realityObject.ProjectDocs,
                        realityObject.EnergyPassport,
                        realityObject.ConfirmWorkDocs,
                        TechPassportScanFile = realityObjectExtension != null ? realityObjectExtension.TechPassportScanFile : null
                    };

            return new BaseDataResult(result);
        }
    }
}