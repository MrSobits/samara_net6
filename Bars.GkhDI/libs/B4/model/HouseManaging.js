Ext.define('B4.model.HouseManaging', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseManaging'
    },
    fields: [
        { name: 'DateStart', defaultValue: null },
        { name: 'DateEnd', defaultValue: null },
        { name: 'ContractFoundation', defaultValue: null },
        { name: 'DocumentName', defaultValue: null },
        { name: 'DocumentDate', defaultValue: null },
        { name: 'DocumentNumber', defaultValue: null },
        { name: 'ContractStopReason', defaultValue: null }
    ]
});