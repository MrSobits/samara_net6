Ext.define('B4.model.HouseService', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'LsCount' },
        { name: 'Service' },
        { name: 'Supplier' },
        { name: 'Formula' }
    ]
});