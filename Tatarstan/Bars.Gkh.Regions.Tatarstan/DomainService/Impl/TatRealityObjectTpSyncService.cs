namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class TatRealityObjectTpSyncService : RealityObjectTpSyncService
    {
        /// <inheritdoc />
        protected override Dictionary<string, string> tpMap => new Dictionary<string, string>
        {
            {"BuildYear", "Form_1#2:1"},
            {"TypeProject", "Form_1#28:1"},
            {"PhysicalWear", "Form_1#20:1"},
            {"TotalBuildingVolume", "Form_1#5:1"},
            {"AreaOwned", "Form_1#24:1"},
            {"AreaMunicipalOwned", "Form_1#25:1"},
            {"AreaGovernmentOwned", "Form_1#26:1"},
            {"AreaLivingNotLivingMkd", "Form_1#6:1"},
            {"AreaLiving", "Form_1#7:1"},
            {"AreaNotLivingFunctional", "Form_1#8:1"},
            {"AreaCommonUsage", "Form_1_3#3:1"},
            {"MaximumFloors", "Form_1#11:1"},
            {"Floors", "Form_1#10:1"},
            {"NumberEntrances", "Form_1#12:1"},
            {"NumberLiving", "Form_1#13:1"},
            {"NumberApartments", "Form_1#35:1"},
            {"NumberNonResidentialPremises", "Form_1#36:1"},
            {"CadastralHouseNumber", "Form_1#29:1"},
            {"PrivatizationDateFirstApartment", "Form_1#31:1"},
            {"IsCulturalHeritage", "Form_1#40:1"},
            {"IsEmergency", "Form_1#41:1"},
            {"HeatingSystem", "Form_3_1#1:3"},
            {"WallMaterial", "Form_5_2#1:3"},
            {"TypeRoof", "Form_5_6#1:3"},
            {"RoofingMaterial", "Form_5_6_2#29:1"},
            {"CadastreNumber", "Form_1#30:1"},
            {"DateCommissioning", "Form_1#47:1" },
            {"NumberLifts", "Form_4_2#1:1"}
        };

        /// <inheritdoc />
        public TatRealityObjectTpSyncService(IDomainService<TehPassportValue> tpService, IRepository<RealityObject> roRepo, IWindsorContainer container)
            : base(tpService, roRepo, container)
        {
        }
    }
}
