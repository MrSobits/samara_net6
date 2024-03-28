Ext.define('B4.model.RealEstateTypePriorityParam', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealEstateTypePriorityParam'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Weight' },
        { name: 'RealEstateType' }
    ]
});