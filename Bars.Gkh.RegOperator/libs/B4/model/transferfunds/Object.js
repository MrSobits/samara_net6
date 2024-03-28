Ext.define('B4.model.transferfunds.Object', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TransferObject',
        timeout: 10 * 60 * 1000 // 10 минут
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'TransferObject', defaultValue: null },
        { name: 'Transferred', defaultValue: false },
        { name: 'TransferredSum' },
        { name: 'PaidTotal' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'GkhCode' }
    ]
});