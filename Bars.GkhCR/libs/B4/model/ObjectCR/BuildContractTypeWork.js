Ext.define('B4.model.objectcr.BuildContractTypeWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuildContractTypeWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BuildContract' },
        { name: 'TypeWork' },
        { name: 'Sum' }
    ]
});