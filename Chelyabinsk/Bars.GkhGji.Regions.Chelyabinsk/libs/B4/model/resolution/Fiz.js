Ext.define('B4.model.resolution.Fiz', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionFiz'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Resolution', defaultValue: null },
        { name: 'PhysicalPersonDocType'},
        { name: 'DocumentNumber' },
        { name: 'DocumentSerial' },
        { name: 'PayerCode' },
        { name: 'IsRF', defaultValue: true }
    ]
});