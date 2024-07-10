//const { debug } = require("console");

Ext.define('B4.aspects.permission.realityobj.RealityObjectFields', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjfieldsperm',
    applyByPostfix: true,
    mainViewSelector: 'realityobjEditPanel',
    init: function () {
        var me = this;

        me.permissions = [
            { name: 'Gkh.RealityObject.Field.View.FiasAddress_View', applyTo: 'field[name=FiasAddress]' },
            { name: 'Gkh.RealityObject.Field.View.FiasHauseGuid_View', applyTo: 'field[name=FiasHauseGuid]' },
            { name: 'Gkh.RealityObject.Field.View.TypeHouse_View', applyTo: 'field[name=TypeHouse]' },
            { name: 'Gkh.RealityObject.Field.View.BuildYear_View', applyTo: 'field[name=BuildYear]' },
            { name: 'Gkh.RealityObject.Field.View.PublishDate_View', applyTo: 'field[name=PublishDate]' },
            { name: 'Gkh.RealityObject.Field.View.DateLastOverhaul_View', applyTo: 'field[name=DateLastOverhaul]' },
            { name: 'Gkh.RealityObject.Field.View.HasPrivatizedFlats_View', applyTo: 'field[name=HasPrivatizedFlats]' },
            { name: 'Gkh.RealityObject.Field.View.DateTechInspection_View', applyTo: 'field[name=DateTechInspection]' },
            { name: 'Gkh.RealityObject.Field.View.MethodFormFundCr_View', applyTo: 'field[name=MethodFormFundCr]' },
            { name: 'Gkh.RealityObject.Field.View.ConditionHouse_View', applyTo: 'field[name=ConditionHouse]' },

            { name: 'Gkh.RealityObject.Field.View.ConditionHouse_View', applyTo: 'changevalbtn[propertyName=ConditionHouse]' },

             {
                 name: 'Gkh.RealityObject.Field.View.ConditionHouse_EditButton', applyTo: 'changevalbtn', selector: 'realityobjectconditionhousecmp',
                 applyBy: function (component, allowed) {
                     if (allowed) {
                         component.show();
                     } else {
                         component.hide();
                     }
                 }
            },
            {
                name: 'Gkh.RealityObject.Buttons.SendToGZHI', applyTo: '#btnPrescr', selector: 'realityobjgeneralinfocontainer',
                applyBy: function (component, allowed) {
                    
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            },

            { name: 'Gkh.RealityObject.Field.View.DateCommissioning_View', applyTo: 'field[name=DateCommissioning]' },
            { name: 'Gkh.RealityObject.Field.View.UnpublishDate_View', applyTo: 'field[name=UnpublishDate]' },
            { name: 'Gkh.RealityObject.Field.View.DateCommissioningLastSection_View', applyTo: 'field[name=DateCommissioningLastSection]' },
            { name: 'Gkh.RealityObject.Field.View.PrivatizationDateFirstApartment_View', applyTo: 'field[name=PrivatizationDateFirstApartment]' },
            { name: 'Gkh.RealityObject.Field.View.LatestTechnicalMonitoring_View', applyTo: 'field[name=LatestTechnicalMonitoring]' },
            { name: 'Gkh.RealityObject.Field.View.ResidentsEvicted_View', applyTo: 'field[name=ResidentsEvicted]' },
            { name: 'Gkh.RealityObject.Field.View.IsCulturalHeritage_View', applyTo: 'field[name=IsCulturalHeritage]' },
            { name: 'Gkh.RealityObject.Field.View.IsInsuredObject_View', applyTo: 'field[name=IsInsuredObject]' },
            { name: 'Gkh.RealityObject.Field.View.IsNotInvolvedCr_View', applyTo: 'field[name=IsNotInvolvedCr]' },
            { name: 'Gkh.RealityObject.Field.View.IsRepairInadvisable_View', applyTo: 'field[name=IsRepairInadvisable]' },
            { name: 'Gkh.RealityObject.Field.View.IsInvolvedCrTo2_View', applyTo: 'field[name=IsInvolvedCrTo2]' },
            { name: 'Gkh.RealityObject.Field.View.DateDemolition_View', applyTo: 'field[name=DateDemolition]' },
            { name: 'Gkh.RealityObject.Field.View.CulturalHeritageAssignmentDate_View', applyTo: 'field[name=CulturalHeritageAssignmentDate]' },
            { name: 'Gkh.RealityObject.Field.View.FederalNum_View', applyTo: 'field[name=FederalNum]' },
            { name: 'Gkh.RealityObject.Field.View.GkhCode_View', applyTo: 'field[name=GkhCode]' },
            { name: 'Gkh.RealityObject.Field.View.AddressCode_View', applyTo: 'field[name=AddressCode]' },
            { name: 'Gkh.RealityObject.Field.View.CodeErc_View', applyTo: 'field[name=CodeErc]' },
            { name: 'Gkh.RealityObject.Field.View.PhysicalWear_View', applyTo: 'field[name=PhysicalWear]' },
            { name: 'Gkh.RealityObject.Field.View.VtscpCode_View', applyTo: 'field[name=VtscpCode]' },
            { name: 'Gkh.RealityObject.Field.View.BuiltOnResettlementProgram_View', applyTo: 'field[name=BuiltOnResettlementProgram]' },
            { name: 'Gkh.RealityObject.Field.View.TechPassportFile_View', applyTo: 'b4filefield[name=TechPassportFile]' },
            { name: 'Gkh.RealityObject.Field.View.BuildingFeature_View', applyTo: 'field[name=BuildingFeature]' },
            { name: 'Gkh.RealityObject.Field.View.IsBuildSocialMortgage_View', applyTo: 'field[name=IsBuildSocialMortgage]' },
            { name: 'Gkh.RealityObject.Field.View.TypeOwnership_View', applyTo: 'field[name=TypeOwnership]' },
            { name: 'Gkh.RealityObject.Field.View.TypeProject_View', applyTo: 'field[name=TypeProject]' },
            { name: 'Gkh.RealityObject.Field.View.RealEstateType_View', applyTo: 'field[name=RealEstateType]' },
            { name: 'Gkh.RealityObject.Field.View.RealEstateTypeNames_View', applyTo: 'field[name=RealEstateTypeNames]' },
            { name: 'Gkh.RealityObject.Field.View.CapitalGroup_View', applyTo: 'field[name=CapitalGroup]' },
            { name: 'Gkh.RealityObject.Field.View.WebCameraUrl_View', applyTo: 'field[name=WebCameraUrl]' },
            { name: 'Gkh.RealityObject.Field.View.ObjectConstruction_View', applyTo: 'field[name=ObjectConstruction]' },
            { name: 'Gkh.RealityObject.Field.View.CentralHeatingStation_View', applyTo: 'field[name=CentralHeatingStation]' },
            { name: 'Gkh.RealityObject.Field.View.AddressCtp_View', applyTo: 'field[name=AddressCtp]' },
            { name: 'Gkh.RealityObject.Field.View.NumberInCtp_View', applyTo: 'field[name=NumberInCtp]' },
            { name: 'Gkh.RealityObject.Field.View.PriorityCtp_View', applyTo: 'field[name=PriorityCtp]' },
            { name: 'Gkh.RealityObject.Field.View.CadastreNumber_View', applyTo: 'field[name=CadastreNumber]' },
            { name: 'Gkh.RealityObject.Field.View.CadastralHouseNumber_View', applyTo: 'field[name=CadastralHouseNumber]' },
            { name: 'Gkh.RealityObject.Field.View.TotalBuildingVolume_View', applyTo: 'field[name=TotalBuildingVolume]' },
            { name: 'Gkh.RealityObject.Field.View.AreaMkd_View', applyTo: 'field[name=AreaMkd]' },
            { name: 'Gkh.RealityObject.Field.View.AreaOwned_View', applyTo: 'field[name=AreaOwned]' },
            { name: 'Gkh.RealityObject.Field.View.AreaMunicipalOwned_View', applyTo: 'field[name=AreaMunicipalOwned]' },
            { name: 'Gkh.RealityObject.Field.View.AreaGovernmentOwned_View', applyTo: 'field[name=AreaGovernmentOwned]' },
            { name: 'Gkh.RealityObject.Field.View.AreaLivingNotLivingMkd_View', applyTo: 'field[name=AreaLivingNotLivingMkd]' },
            { name: 'Gkh.RealityObject.Field.View.AreaLiving_View', applyTo: 'field[name=AreaLiving]' },
            { name: 'Gkh.RealityObject.Field.View.AreaNotLivingPremises_View', applyTo: 'field[name=AreaNotLivingPremises]' },
            { name: 'Gkh.RealityObject.Field.View.AreaLivingOwned_View', applyTo: 'field[name=AreaLivingOwned]' },
            { name: 'Gkh.RealityObject.Field.View.AreaNotLivingFunctional_View', applyTo: 'field[name=AreaNotLivingFunctional]' },
            { name: 'Gkh.RealityObject.Field.View.AreaCommonUsage_View', applyTo: 'field[name=AreaCommonUsage]' },
            { name: 'Gkh.RealityObject.Field.View.AreaCleaning_View', applyTo: 'field[name=AreaCleaning]' },
            { name: 'Gkh.RealityObject.Field.View.NecessaryConductCr_View', applyTo: 'field[name=NecessaryConductCr]' },
            { name: 'Gkh.RealityObject.Field.View.MaximumFloors_View', applyTo: 'field[name=MaximumFloors]' },
            { name: 'Gkh.RealityObject.Field.View.Floors_View', applyTo: 'field[name=Floors]' },
            { name: 'Gkh.RealityObject.Field.View.FloorHeight_View', applyTo: 'field[name=FloorHeight]' },
            { name: 'Gkh.RealityObject.Field.View.NumberEntrances_View', applyTo: 'field[name=NumberEntrances]' },
            { name: 'Gkh.RealityObject.Field.View.NumberApartments_View', applyTo: 'field[name=NumberApartments]' },
            { name: 'Gkh.RealityObject.Field.View.NumberNonResidentialPremises_View', applyTo: 'field[name=NumberNonResidentialPremises]' },
            { name: 'Gkh.RealityObject.Field.View.NumberLiving_View', applyTo: 'field[name=NumberLiving]' },
            { name: 'Gkh.RealityObject.Field.View.NumberLifts_View', applyTo: 'field[name=NumberLifts]' },
            { name: 'Gkh.RealityObject.Field.View.RoofingMaterial_View', applyTo: 'field[name=RoofingMaterial]' },
            { name: 'Gkh.RealityObject.Field.View.WallMaterial_View', applyTo: 'field[name=WallMaterial]' },
            { name: 'Gkh.RealityObject.Field.View.TypeRoof_View', applyTo: 'field[name=TypeRoof]' },
            { name: 'Gkh.RealityObject.Field.View.PercentDebt_View', applyTo: 'field[name=PercentDebt]' },
            { name: 'Gkh.RealityObject.Field.View.HeatingSystem_View', applyTo: 'field[name=HeatingSystem]' },
            { name: 'Gkh.RealityObject.Field.View.HasJudgmentCommonProp_View', applyTo: 'field[name=HasJudgmentCommonProp]' },
            { name: 'Gkh.RealityObject.Field.View.TechPassportScanFile_View', applyTo: 'field[name=TechPassportScanFile]' },
            { name: 'Gkh.RealityObject.Field.View.Notation_View', applyTo: 'field[name=Notation]' },
            { name: 'Gkh.RealityObject.Field.View.MonumentDocumentNumber_View', applyTo: 'field[name=MonumentDocumentNumber]' },
            { name: 'Gkh.RealityObject.Field.View.MonumentFile_View', applyTo: 'field[name=MonumentFile]' },
            { name: 'Gkh.RealityObject.Field.View.MonumentDepartmentName_View', applyTo: 'field[name=MonumentDepartmentName]' },
            { name: 'Gkh.RealityObject.Field.View.IsIncludedRegisterCHO_View', applyTo: 'field[name=IsIncludedRegisterCHO]' },
            { name: 'Gkh.RealityObject.Field.View.IsIncludedListIdentifiedCHO_View', applyTo: 'field[name=IsIncludedListIdentifiedCHO]' },
            { name: 'Gkh.RealityObject.Field.View.IsDeterminedSubjectProtectionCHO_View', applyTo: 'field[name=IsDeterminedSubjectProtectionCHO]' },

            { name: 'Gkh.RealityObject.Field.Edit.FiasAddress_Edit', applyTo: 'field[name=FiasAddress]' },
            { name: 'Gkh.RealityObject.Field.Edit.TypeHouse_Edit', applyTo: 'field[name=TypeHouse]' },
            { name: 'Gkh.RealityObject.Field.Edit.BuildYear_Edit', applyTo: 'field[name=BuildYear]' },
            { name: 'Gkh.RealityObject.Field.Edit.PublishDate_Edit', applyTo: 'field[name=PublishDate]' },
            { name: 'Gkh.RealityObject.Field.Edit.DateLastOverhaul_Edit', applyTo: 'field[name=DateLastOverhaul]' },
            { name: 'Gkh.RealityObject.Field.Edit.HasPrivatizedFlats_Edit', applyTo: 'field[name=HasPrivatizedFlats]' },
            { name: 'Gkh.RealityObject.Field.Edit.DateTechInspection_Edit', applyTo: 'field[name=DateTechInspection]' },
            { name: 'Gkh.RealityObject.Field.Edit.MethodFormFundCr_Edit', applyTo: 'field[name=MethodFormFundCr]' },
            { name: 'Gkh.RealityObject.Field.Edit.ConditionHouse_Edit', applyTo: 'field[name=ConditionHouse]' },
            {
                name: 'Gkh.RealityObject.Field.Edit.ConditionHouse_Edit',
                applyTo: 'changevalbtn[propertyName=ConditionHouse]',
                applyBy: function (component, allowed) {
                    if (component) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            },
            { name: 'Gkh.RealityObject.Field.Edit.DateCommissioning_Edit', applyTo: 'field[name=DateCommissioning]' },
            { name: 'Gkh.RealityObject.Field.Edit.UnpublishDate_Edit', applyTo: 'field[name=UnpublishDate]' },
            { name: 'Gkh.RealityObject.Field.Edit.DateCommissioningLastSection_Edit', applyTo: 'field[name=DateCommissioningLastSection]' },
            { name: 'Gkh.RealityObject.Field.Edit.PrivatizationDateFirstApartment_Edit', applyTo: 'field[name=PrivatizationDateFirstApartment]' },
            { name: 'Gkh.RealityObject.Field.Edit.LatestTechnicalMonitoring_Edit', applyTo: 'field[name=LatestTechnicalMonitoring]' },
            { name: 'Gkh.RealityObject.Field.Edit.ResidentsEvicted_Edit', applyTo: 'field[name=ResidentsEvicted]' },
            { name: 'Gkh.RealityObject.Field.Edit.IsCulturalHeritage_Edit', applyTo: 'field[name=IsCulturalHeritage]' },
            { name: 'Gkh.RealityObject.Field.Edit.IsInsuredObject_Edit', applyTo: 'field[name=IsInsuredObject]' },
            { name: 'Gkh.RealityObject.Field.Edit.IsNotInvolvedCr_Edit', applyTo: 'field[name=IsNotInvolvedCr]' },
            { name: 'Gkh.RealityObject.Field.Edit.IsRepairInadvisable_Edit', applyTo: 'field[name=IsRepairInadvisable]' },
            { name: 'Gkh.RealityObject.Field.Edit.IsInvolvedCrTo2_Edit', applyTo: 'field[name=IsInvolvedCrTo2]' },
            { name: 'Gkh.RealityObject.Field.Edit.DateDemolition_Edit', applyTo: 'field[name=DateDemolition]' },
            { name: 'Gkh.RealityObject.Field.Edit.CulturalHeritageAssignmentDate_Edit', applyTo: 'field[name=CulturalHeritageAssignmentDate]' },
            { name: 'Gkh.RealityObject.Field.Edit.FederalNum_Edit', applyTo: 'field[name=FederalNum]' },
            { name: 'Gkh.RealityObject.Field.Edit.GkhCode_Edit', applyTo: 'field[name=GkhCode]' },
            { name: 'Gkh.RealityObject.Field.Edit.AddressCode_Edit', applyTo: 'field[name=AddressCode]' },
            { name: 'Gkh.RealityObject.Field.Edit.CodeErc_Edit', applyTo: 'field[name=CodeErc]' },
            { name: 'Gkh.RealityObject.Field.Edit.PhysicalWear_Edit', applyTo: 'field[name=PhysicalWear]' },
            { name: 'Gkh.RealityObject.Field.Edit.VtscpCode_Edit', applyTo: 'field[name=VtscpCode]' },
            { name: 'Gkh.RealityObject.Field.Edit.BuiltOnResettlementProgram_Edit', applyTo: 'field[name=BuiltOnResettlementProgram]' },
            { name: 'Gkh.RealityObject.Field.Edit.BuildingFeature_Edit', applyTo: 'field[name=BuildingFeature]' },
            { name: 'Gkh.RealityObject.Field.Edit.IsBuildSocialMortgage_Edit', applyTo: 'field[name=IsBuildSocialMortgage]' },
            { name: 'Gkh.RealityObject.Field.Edit.TypeOwnership_Edit', applyTo: 'field[name=TypeOwnership]' },
            { name: 'Gkh.RealityObject.Field.Edit.TypeProject_Edit', applyTo: 'field[name=TypeProject]' },
            { name: 'Gkh.RealityObject.Field.Edit.RealEstateType_Edit', applyTo: 'field[name=RealEstateType]' },
            { name: 'Gkh.RealityObject.Field.Edit.RealEstateTypeNames_Edit', applyTo: 'field[name=RealEstateTypeNames]' },
            { name: 'Gkh.RealityObject.Field.Edit.CapitalGroup_Edit', applyTo: 'field[name=CapitalGroup]' },
            { name: 'Gkh.RealityObject.Field.Edit.WebCameraUrl_Edit', applyTo: 'field[name=WebCameraUrl]' },
            { name: 'Gkh.RealityObject.Field.Edit.ObjectConstruction_Edit', applyTo: 'field[name=ObjectConstruction]' },
            { name: 'Gkh.RealityObject.Field.Edit.CentralHeatingStation_Edit', applyTo: 'field[name=CentralHeatingStation]' },
            { name: 'Gkh.RealityObject.Field.Edit.AddressCtp_Edit', applyTo: 'field[name=AddressCtp]' },
            { name: 'Gkh.RealityObject.Field.Edit.NumberInCtp_Edit', applyTo: 'field[name=NumberInCtp]' },
            { name: 'Gkh.RealityObject.Field.Edit.PriorityCtp_Edit', applyTo: 'field[name=PriorityCtp]' },
            { name: 'Gkh.RealityObject.Field.Edit.CadastreNumber_Edit', applyTo: 'field[name=CadastreNumber]' },
            { name: 'Gkh.RealityObject.Field.Edit.CadastralHouseNumber_Edit', applyTo: 'field[name=CadastralHouseNumber]' },
            { name: 'Gkh.RealityObject.Field.Edit.TotalBuildingVolume_Edit', applyTo: 'field[name=TotalBuildingVolume]' },
            { name: 'Gkh.RealityObject.Field.Edit.AreaMkd_Edit', applyTo: 'field[name=AreaMkd]' },
            { name: 'Gkh.RealityObject.Field.Edit.AreaOwned_Edit', applyTo: 'field[name=AreaOwned]' },
            { name: 'Gkh.RealityObject.Field.Edit.AreaMunicipalOwned_Edit', applyTo: 'field[name=AreaMunicipalOwned]' },
            { name: 'Gkh.RealityObject.Field.Edit.AreaGovernmentOwned_Edit', applyTo: 'field[name=AreaGovernmentOwned]' },
            { name: 'Gkh.RealityObject.Field.Edit.AreaLivingNotLivingMkd_Edit', applyTo: 'field[name=AreaLivingNotLivingMkd]' },
            { name: 'Gkh.RealityObject.Field.Edit.AreaLiving_Edit', applyTo: 'field[name=AreaLiving]' },
            { name: 'Gkh.RealityObject.Field.Edit.AreaNotLivingPremises_Edit', applyTo: 'field[name=AreaNotLivingPremises]' },
            { name: 'Gkh.RealityObject.Field.Edit.AreaLivingOwned_Edit', applyTo: 'field[name=AreaLivingOwned]' },
            { name: 'Gkh.RealityObject.Field.Edit.AreaNotLivingFunctional_Edit', applyTo: 'field[name=AreaNotLivingFunctional]' },
            { name: 'Gkh.RealityObject.Field.Edit.AreaCommonUsage_Edit', applyTo: 'field[name=AreaCommonUsage]' },
            { name: 'Gkh.RealityObject.Field.Edit.AreaCleaning_Edit', applyTo: 'field[name=AreaCleaning]' },
            { name: 'Gkh.RealityObject.Field.Edit.NecessaryConductCr_Edit', applyTo: 'field[name=NecessaryConductCr]' },
            { name: 'Gkh.RealityObject.Field.Edit.MaximumFloors_Edit', applyTo: 'field[name=MaximumFloors]' },
            { name: 'Gkh.RealityObject.Field.Edit.Floors_Edit', applyTo: 'field[name=Floors]' },
            { name: 'Gkh.RealityObject.Field.Edit.FloorHeight_Edit', applyTo: 'field[name=FloorHeight]' },
            { name: 'Gkh.RealityObject.Field.Edit.NumberEntrances_Edit', applyTo: 'field[name=NumberEntrances]' },
            { name: 'Gkh.RealityObject.Field.Edit.NumberApartments_Edit', applyTo: 'field[name=NumberApartments]' },
            { name: 'Gkh.RealityObject.Field.Edit.NumberNonResidentialPremises_Edit', applyTo: 'field[name=NumberNonResidentialPremises]' },
            { name: 'Gkh.RealityObject.Field.Edit.NumberLiving_Edit', applyTo: 'field[name=NumberLiving]' },
            { name: 'Gkh.RealityObject.Field.Edit.NumberLifts_Edit', applyTo: 'field[name=NumberLifts]' },
            { name: 'Gkh.RealityObject.Field.Edit.RoofingMaterial_Edit', applyTo: 'field[name=RoofingMaterial]' },
            { name: 'Gkh.RealityObject.Field.Edit.WallMaterial_Edit', applyTo: 'field[name=WallMaterial]' },
            { name: 'Gkh.RealityObject.Field.Edit.TypeRoof_Edit', applyTo: 'field[name=TypeRoof]' },
            { name: 'Gkh.RealityObject.Field.Edit.PercentDebt_Edit', applyTo: 'field[name=PercentDebt]' },
            { name: 'Gkh.RealityObject.Field.Edit.HeatingSystem_Edit', applyTo: 'field[name=HeatingSystem]' },
            { name: 'Gkh.RealityObject.Field.Edit.HasJudgmentCommonProp_Edit', applyTo: 'field[name=HasJudgmentCommonProp]' },
            { name: 'Gkh.RealityObject.Field.Edit.TechPassportScanFile_Edit', applyTo: 'field[name=TechPassportScanFile]' },
            { name: 'Gkh.RealityObject.Field.Edit.Notation_Edit', applyTo: 'field[name=Notation]' },
            { name: 'Gkh.RealityObject.Field.Edit.MonumentDocumentNumber_Edit', applyTo: 'field[name=MonumentDocumentNumber]' },
            { name: 'Gkh.RealityObject.Field.Edit.MonumentFile_Edit', applyTo: 'field[name=MonumentFile]' },
            { name: 'Gkh.RealityObject.Field.Edit.MonumentDepartmentName_Edit', applyTo: 'field[name=MonumentDepartmentName]' },
            { name: 'Gkh.RealityObject.Field.Edit.IsIncludedRegisterCHO_Edit', applyTo: 'field[name=IsIncludedRegisterCHO]' },
            { name: 'Gkh.RealityObject.Field.Edit.IsIncludedListIdentifiedCHO_Edit', applyTo: 'field[name=IsIncludedListIdentifiedCHO]' },
            { name: 'Gkh.RealityObject.Field.Edit.IsDeterminedSubjectProtectionCHO_Edit', applyTo: 'field[name=IsDeterminedSubjectProtectionCHO]' }
        ];

        me.callParent(arguments);
    }
});