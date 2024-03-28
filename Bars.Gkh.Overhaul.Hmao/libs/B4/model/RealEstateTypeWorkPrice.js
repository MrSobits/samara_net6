Ext.define('B4.model.RealEstateTypeWorkPrice', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealEstateTypeWorkPrice'
    },
    fields: [
        { name: 'Id', type: 'number', defaultValue: 0 },
        { name: 'WorkPrice' },
        { name: 'RealEstateType'}
    ]

});