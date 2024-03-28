Ext.define('B4.model.objectcr.MassBuildContractWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MassBuildContractWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MassBuildContract' },
        { name: 'Work' },
        { name: 'Sum' }
    ]
});