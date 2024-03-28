Ext.define('B4.model.objectcr.ContractCrTypeWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContractCrTypeWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ContractCr' },
        { name: 'TypeWork' },
        { name: 'Sum' }
    ]
});