Ext.define('B4.model.objectcr.MassBuildContractObjectCrWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MassBuildContractObjectCrWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MassBuildContractObjectCr' },
        { name: 'Work' },
        { name: 'Sum' }
    ]
});