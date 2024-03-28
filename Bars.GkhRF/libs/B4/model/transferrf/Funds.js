Ext.define('B4.model.transferrf.Funds', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TransferFundsRf'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RequestTransferRf', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'RealityObjectName', defaultValue: null },
        { name: 'WorkKind' },
        { name: 'PayAllocate' },
        { name: 'PersonalAccount' },
        { name: 'Sum', defaultValue: null }
    ]
});
