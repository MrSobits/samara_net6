﻿﻿Ext.define('B4.model.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [
        'B4.enums.YesNoNotSet',
        'B4.enums.HeatingSystem',
        'B4.enums.ConditionHouse',
        'B4.enums.TypeHouse',
        'B4.enums.TypeRoof',
        'B4.enums.YesNo',
        'B4.enums.TypePresence'
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'realityobject',
        timeout: 15 * 60 * 1000 // 15 минут для сохранения
    },
    fields: [
        { name: 'ExternalId', defaultValue: null },
        { name: 'Id', useNull: true },
        { name: 'Municipality', defaultValue: null },
        { name: 'Settlement', defaultValue: null },
        { name: 'FiasAddress' },
        { name: 'FiasHauseGuid' },
        { name: 'Address' },
        { name: 'FullAddress' },
        { name: 'CodeErc', defaultValue: null },
        { name: 'AreaLiving' },
        { name: 'Iscluttered' },
        { name: 'HasVidecam' },
        { name: 'AreaLivingOwned' },
        { name: 'AreaLivingNotLivingMkd' },
        { name: 'AreaMkd' },
        { name: 'AreaCommonUsage' },
        { name: 'AreaCleaning' },
        { name: 'AreaBasement' },
        { name: 'DateLastOverhaul' },
        { name: 'DateCommissioning' },
        { name: 'CapitalGroup', defaultValue: null },
        { name: 'DateDemolition' },
        { name: 'MaximumFloors' },
        { name: 'RoofingMaterial', defaultValue: null },
        { name: 'WallMaterial', defaultValue: null },
        { name: 'IsInsuredObject', defaultValue: false },
        { name: 'IsCulturalHeritage', defaultValue: false },
        { name: 'Notation' },
        { name: 'SeriesHome' },
        { name: 'PhysicalWear' },
        { name: 'TypeOwnership', defaultValue: null },
        { name: 'Floors' },
        { name: 'FederalNum' },
        { name: 'NumberApartments' },
        { name: 'NumberEntrances' },
        { name: 'NumberLifts' },
        { name: 'NumberLiving' },
        { name: 'HavingBasement', defaultValue: 30 },
        { name: 'HeatingSystem', defaultValue: 20 },
        { name: 'ConditionHouse', defaultValue: 30 },
        { name: 'TypeHouse', defaultValue: 30 },
        { name: 'TypeRoof', defaultValue: 10 },
        { name: 'RoofingMaterialName' },
        { name: 'WallMaterialName' },
        { name: 'RoofingMaterialId' },
        { name: 'WallMaterialId' },
        { name: 'HouseGuid' },
        { name: 'Latitude', defaultValue: 0 },
        { name: 'Longitude', defaultValue: 0 },
        { name: 'ManOrgNames' },
        { name: 'TypeContracts' },
        { name: 'WebCameraUrl' },
        { name: 'DateTechInspection' },
        { name: 'ResidentsEvicted', defaultValue: false },
        { name: 'TypeProject', defaultValue: false },
        { name: 'GkhCode' },
        { name: 'AddressCode' },
        { name: 'IsBuildSocialMortgage', defaultValue: 20 },
        { name: 'AreaOwned' },
        { name: 'AreaMunicipalOwned' },
        { name: 'AreaGovernmentOwned' },
        { name: 'AreaFederalOwned' },
        { name: 'AreaCommercialOwned' },
        { name: 'AreaNotLivingFunctional' },
        { name: 'TotalBuildingVolume' },
        { name: 'CadastreNumber' },
        { name: 'CadastralHouseNumber' },
        { name: 'FloorHeight' },
        { name: 'PrivatizationDateFirstApartment' },
        { name: 'BuildYear' },
        { name: 'State', defaultValue: null },
        { name: 'PercentDebt' },
        { name: 'BuildingFeature' },
        { name: 'RealEstateType' },
        { name: 'NecessaryConductCr', defaultValue: 30 },
        { name: 'MethodFormFundCr', defaultValue: 0 },
        { name: 'HasJudgmentCommonProp', defaultValue: 20 },
        { name: 'IsRepairInadvisable', defaultValue: false },
        { name: 'HasPrivatizedFlats', defaultValue: false},
        { name: 'ProjectDocs', defaultValue: 10 },
        { name: 'EnergyPassport', defaultValue: 10 },
        { name: 'ConfirmWorkDocs', defaultValue: 10 },
        { name: 'IsNotInvolvedCr', defaultValue: false },
        { name: 'IsInvolvedCr', defaultValue: true },
        { name: 'IsAutoRealEstType', defaultValue: false },
        { name: 'AutoRealEstType' },
        { name: 'District' },
        { name: 'UnomCode' },
        { name: 'PublishDate' },
        { name: 'UnpublishDate' },
        { name: 'CulturalHeritageAssignmentDate' },
        { name: 'VtscpCode' },
        { name: 'Points' },
        { name: 'NumberNonResidentialPremises' },
        { name: 'AreaNotLivingPremises' },
        { name: 'IsInvolvedCrTo2', defaultValue: null },
        { name: 'IsSubProgram' },
        { name: 'IncludeToSubProgram' },
        { name: 'ASSberbankClient' },
        { name: 'LatestTechnicalMonitoring' },
        { name: 'MonumentDocumentNumber' },
        { name: 'MonumentFile' },
        { name: 'MonumentDepartmentName' },
        { name: 'DateCommissioningLastSection' },
        { name: 'HasCharges185FZ' },
        { name: 'Inn' },
        { name: 'StartControlDate' },
        { name: 'IsIncludedRegisterCHO' },
        { name: 'IsIncludedListIdentifiedCHO' },
        { name: 'IsDeterminedSubjectProtectionCHO' },

        //regop
        { name: 'IsCulturalHeritage', defaultValue: false },
        { name: 'PlaceName', defaultValue: null },
        { name: 'RealEstateTypeNames' },
        //tat
        { name: 'ObjectConstruction', defaultValue: 30 },
        { name: 'BuiltOnResettlementProgram', defaultValue: 30 },
        { name: 'CentralHeatingStation' },
        { name: 'AddressCtp' },
        { name: 'NumberInCtp' },
        { name: 'PriorityCtp' },
        { name: 'TechPassportFile', defaultValue: null },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'ObligationDate' },
        //yanao
        { name: 'TechPassportScanFile' },
        { name: 'ExportedToPortal' },

        { name: 'FileInfo' }
    ]
});