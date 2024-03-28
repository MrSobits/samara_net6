Ext.define('B4.model.licensing.GovernmenServiceDetail', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GovernmenServiceDetail'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Value' },
        { name: 'DisplayValue' },
        { name: 'ServiceDetailSectionType' },
        { name: 'InnerDescriptors' }
    ]
});