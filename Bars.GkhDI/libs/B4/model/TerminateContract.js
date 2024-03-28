Ext.define('B4.model.TerminateContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TerminateContract'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TerminateReason' },
        { name: 'AddressName' }
    ]
});