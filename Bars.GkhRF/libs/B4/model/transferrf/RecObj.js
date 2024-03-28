Ext.define('B4.model.transferrf.RecObj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TransferRfRecObj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TransferRfRecord', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'TransferRfRecordName'},
        { name: 'RealityObjectName' },
        { name: 'MunicipalityName' },
        { name: 'GkhCode' },
        { name: 'Sum', defaultValue: null }
    ]
});