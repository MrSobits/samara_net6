Ext.define('B4.model.normconsumption.NormConsumptionHeating', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NormConsumptionHeating',
        listAction: 'ListNormConsumptionHeating'
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
        { name: 'GenerealBuildingHeatMeters' },
        { name: 'TechnicalCapabilityOpu' },
        { name: 'AreaHouse' },
        { name: 'AreaLivingRooms' },
        { name: 'AreaNotLivingRooms' },
        { name: 'AreaOtherRooms' },
        { name: 'IsIpuNotLivingPermises' },
        { name: 'HeatEnergyConsumptionInPeriod' },
        { name: 'HotWaterConsumptionInPeriod' },
        { name: 'TypeHotWaterSystemStr' },
        { name: 'TypeHotWaterSystem' },
        { name: 'IsHeatedTowelRail' },
        { name: 'Risers' },
        { name: 'HeatEnergyConsumptionNotLivInPeriod' },
        { name: 'HotWaterConsumptionNotLivInPeriod' },
        { name: 'HeatingPeriod' },
        { name: 'AvgTempColdWater' },
        { name: 'WearIntrahouseUtilites' },
        { name: 'OverhaulDate' }
    ]
});
