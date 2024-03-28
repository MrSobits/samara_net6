Ext.define('B4.model.gisrealestate.realestatetype.RealEstateTypeCommonParam', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisRealEstateTypeCommonParam'
    },
    fields: [
        { name: 'Id', type: 'number', defaultValue: 0 },
        { name: 'RealEstateType' },
        { name: 'CommonParamCode' },
        { name: 'Min' },
        { name: 'Max' },
        { name: 'IsPrecision' },
        { name: 'PrecisionValue' }
    ]
});