Ext.define('B4.aspects.fieldrequirement.RealityObject', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.realityobjfieldrequirement',

    init: function () {
        this.requirements = [

            { name: 'Gkh.RealityObject.Field.Address_Rqrd', applyTo: '#fiasAddressRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.HauseGuid_Rqrd', applyTo: '#fiasHauseGuidRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.TypeHouse_Rqrd', applyTo: '#cbTypeHouseRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.ConditionHouse_Rqrd', applyTo: '#cbConditionHouseRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.BuildYear_Rqrd', applyTo: '#nfBuildYear', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.DateDemolition_Rqrd', applyTo: '#dfDateDemolutionRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.DateCommissioning_Rqrd', applyTo: '#dfDateComissioningRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.DateCommissioningLastSection_Rqrd', applyTo: '#dfDateComissioningRealityObjectLastSection', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.DateTechInspection_Rqrd', applyTo: '#dfDateTechInspectionRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.PrivatizationDateFirstApartment_Rqrd', applyTo: '#dfPrivatizationDateFirstApartment', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.PrivatizationDateFirstApartment_Rqrd', applyTo: '#hasPrivatizedFlats', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.MethodFormFundCr_Rqrd', applyTo: '#cbMethodFormFundCr', selector: 'realityobjEditPanel' },

            { name: 'Gkh.RealityObject.Field.FederalNum_Rqrd', applyTo: '#tfFederalNumRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.GkhCode_Rqrd', applyTo: '#tfGkhCodeRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.TypeProject_Rqrd', applyTo: '#sfTypeProject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.CapitalGroup_Rqrd', applyTo: '#sfCapitalGroupRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.WebCameraUrl_Rqrd', applyTo: '#tfWebCameraUrlRealityObject', selector: 'realityobjEditPanel' },

            { name: 'Gkh.RealityObject.Field.TypeOwnership_Rqrd', applyTo: '#sfTypeOwnershipRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.PhysicalWear_Rqrd', applyTo: '#nfPhysicalWearRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.BuildSocialMortgage_Rqrd', applyTo: '#cbIsBuildSocialMortgageRealObj', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.CodeErc_Rqrd', applyTo: '#tfCodeErcRealityObject', selector: 'realityobjEditPanel' },

            { name: 'Gkh.RealityObject.Field.CadastreNumber_Rqrd', applyTo: '#tfCadastreNumber', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.CadastralHouseNumber_Rqrd', applyTo: '#tfCadastralHouseNumber', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.TotalBuildingVolume_Rqrd', applyTo: '#nfTotalBuildingVolume', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.AreaMkd_Rqrd', applyTo: '[name=AreaMkd]', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.AreaOwned_Rqrd', applyTo: '#nfAreaOwned', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.AreaMunicipalOwned_Rqrd', applyTo: '#nfAreaMunicipalOwned', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.AreaGovernmentOwned_Rqrd', applyTo: '#nfAreaGovernmentOwned', selector: 'realityobjEditPanel' },

            { name: 'Gkh.RealityObject.Field.AreaCommercialOwned_Rqrd', applyTo: '#nfAreaCommercialOwned', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.AreaFederalOwned_Rqrd', applyTo: '#nfAreaFederalOwned', selector: 'realityobjEditPanel' },

            { name: 'Gkh.RealityObject.Field.AreaLivingNotLivingMkd_Rqrd', applyTo: '#nfAreaLivingNotLivingMkdRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.AreaLiving_Rqrd', applyTo: '#nfAreaLivingRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.AreaLivingOwned_Rqrd', applyTo: '#nfAreaLivingOwnedRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.AreaNotLivingFunctional_Rqrd', applyTo: '#nfAreaNotLivingFunctional', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.NecessaryConductCr_Rqrd', applyTo: '#cbNecessaryConductCr', selector: 'realityobjEditPanel' },

            { name: 'Gkh.RealityObject.Field.MaximumFloors_Rqrd', applyTo: '#nfMaximumFloorsRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.Floors_Rqrd', applyTo: '#nfFloorsRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.FloorHeight_Rqrd', applyTo: '#nfFloorHeight', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.NumberEntrances_Rqrd', applyTo: '#nfNumberEntrancesRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.NumberApartments_Rqrd', applyTo: '#nfNumberApartmentsRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.NumberLiving_Rqrd', applyTo: '#nfNumberLivingRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.NumberLifts_Rqrd', applyTo: '#nfNumberLiftsRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.RoofingMaterial_Rqrd', applyTo: '#sfRoofingMaterial', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.WallMaterial_Rqrd', applyTo: '#sfWallMaterialRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.TypeRoof_Rqrd', applyTo: '#cbTypeRoofRealityObject', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.PercentDebt_Rqrd', applyTo: '#nfPercentDebt', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.HeatingSystem_Rqrd', applyTo: '#cbHeatingSystem', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.HasJudgmentCommonProp_Rqrd', applyTo: '#cbHasJudgment', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.LatestTechnicalMonitoring_Rqrd', applyTo: '#dfLatestTechnicalMonitoringRealityObject', selector: 'realityobjEditPanel' },

            { name: 'Gkh.RealityObject.Field.MonumentDocumentNumber_Rqrd', applyTo: 'field[name=MonumentDocumentNumber]', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.MonumentFile_Rqrd', applyTo: 'field[name=MonumentFile]', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.MonumentDepartmentName_Rqrd', applyTo: 'field[name=MonumentDepartmentName]', selector: 'realityobjEditPanel' },

            { name: 'Gkh.RealityObject.Field.DateLastOverhaul_Rqrd', applyTo: 'field[name=DateLastOverhaul]', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.AreaNotLivingPremises_Rqrd', applyTo: 'field[name=AreaNotLivingPremises]', selector: 'realityobjEditPanel' },
            { name: 'Gkh.RealityObject.Field.AreaCommonUsage_Rqrd', applyTo: 'field[name=AreaCommonUsage]', selector: 'realityobjEditPanel' }
        ];

        this.callParent(arguments);
    }
});