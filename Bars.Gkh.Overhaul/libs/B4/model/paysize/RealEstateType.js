Ext.define('B4.model.paysize.RealEstateType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaysizeRealEstateType'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Record', defaultValue: null },
        { name: 'RealEstateType', defaultValue: null },
        { name: 'Name' },
        { name: 'Value' }
    ]
});