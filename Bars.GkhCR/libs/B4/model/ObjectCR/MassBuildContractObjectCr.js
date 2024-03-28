Ext.define('B4.model.objectcr.MassBuildContractObjectCr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MassBuildContractObjectCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MassBuildContract' },
        { name: 'ObjectCr' },
        { name: 'ObjectCrName' },
        { name: 'Municipality' },
        { name: 'Sum' }
    ]
});