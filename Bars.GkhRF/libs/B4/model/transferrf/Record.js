Ext.define('B4.model.transferrf.Record', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TransferRfRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TransferRf', defaultValue: null },
        { name: 'DocumentNum' },
        { name: 'DocumentName', defaultValue: null },
        { name: 'DateFrom', defaultValue: null },
        { name: 'TransferDate', defaultValue: null },
        { name: 'Description' },
        { name: 'CountRecords' },
        { name: 'SumRecords' },
        { name: 'State', defaultValue: null },
        { name: 'File', defaultValue: null }
    ]
});