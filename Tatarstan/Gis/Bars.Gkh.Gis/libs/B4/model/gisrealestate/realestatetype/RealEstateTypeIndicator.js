Ext.define('B4.model.gisrealestate.realestatetype.RealEstateTypeIndicator', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisRealEstateTypeIndicator'
    },
    fields: [
        { name: 'Id', type: 'number', defaultValue: 0 },
        { name: 'RealEstateType' },
        { name: 'RealEstateIndicator' },
        { name: 'RealEstateIndicatorName', mapping: 'RealEstateIndicator.Name' },
        { name: 'Min' },
        { name: 'Max' },
        { name: 'PrecisionValue' }
    ]
});