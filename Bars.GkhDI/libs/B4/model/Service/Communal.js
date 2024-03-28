Ext.define('B4.model.service.Communal', {
    extend: 'B4.model.service.Base',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CommunalService'
    },
    fields: [
        { name: 'TypeOfProvisionService', defaultValue: 10 },
        { name: 'VolumePurchasedResources', defaultValue: null },
        { name: 'PricePurchasedResources', defaultValue: null },
        { name: 'KindElectricitySupply', defaultValue: 10 },
        { name: 'ConsumptionNorms', defaultValue: 0 },
        { name: 'ConsumptionNormLivingHouse', defaultValue: null },
        { name: 'UnitMeasureLivingHouse', defaultValue: null },
        { name: 'AdditionalInfoLivingHouse', defaultValue: null },
        { name: 'ConsumptionNormHousing', defaultValue: null },
        { name: 'UnitMeasureHousing', defaultValue: null },
        { name: 'AdditionalInfoHousing', defaultValue: null }
    ]
});