Ext.define('B4.model.RegopServiceLog', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegopServiceLog'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CashPayCenterName' },
        { name: 'DateExecute' },
        { name: 'FileNum' },
        { name: 'FileDate' },
        { name: 'MethodType' },
        { name: 'Status' },
        { name: 'File' }
    ]
});