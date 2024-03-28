Ext.define('B4.model.dict.ShareFinancingCeo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ShareFinancingCeo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Share', defaultValue: 0 },
        { name: 'CommonEstateObject', defaultValue: null }
    ]
});