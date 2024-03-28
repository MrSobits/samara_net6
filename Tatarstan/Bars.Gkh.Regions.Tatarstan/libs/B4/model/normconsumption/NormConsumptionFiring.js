Ext.define('B4.model.normconsumption.NormConsumptionFiring', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NormConsumptionFiring',
        listAction: 'ListNormConsumptionFiring'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'NormConsumption' },
        { name: 'Municipality' },
        { name: 'RealityObject' },
        { name: 'Address' },
        { name: 'ObjectId' },
        { name: 'FloorNumber' },
        { name: 'BuildYear' },
        { name: 'GenerealBuildingFiringMeters' },
        { name: 'TechnicalCapabilityOpu' },
        { name: 'AreaHouse' },
        { name: 'AreaLivingRooms' },
        { name: 'AreaNotLivingRooms' },
        { name: 'AreaOtherRooms' },
        { name: 'IsIpuNotLivingPermises' },
        { name: 'AreaNotLivingIpu' },
        { name: 'AmountHeatEnergyNotLivingIpu' },
        { name: 'AmountHeatEnergyNotLivInPeriod' },
        { name: 'HeatingPeriod' },
        { name: 'WallMaterial' },
        { name: 'RoofMaterial' },
        { name: 'HourlyHeatLoadForPassport' },
        { name: 'HourlyHeatLoadForDocumentation' },
        { name: 'WearIntrahouseUtilites' },
        { name: 'OverhaulDate' }
    ]
});

