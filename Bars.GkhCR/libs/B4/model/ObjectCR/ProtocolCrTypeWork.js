Ext.define('B4.model.objectcr.ProtocolCrTypeWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolCrTypeWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Protocol', defaultValue: null },
        { name: 'TypeWork', defaultValue: null },
        { name: 'FinanceSourceName' },
        { name: 'WorkName' }
    ]
});