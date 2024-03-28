Ext.define('B4.model.objectcr.DocumentWorkCr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DocumentWorkCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCr' },
        { name: 'Contragent' },
        { name: 'ContragentName' },
        { name: 'DocumentName' },
        { name: 'DocumentNum' },
        { name: 'DateFrom' },
        { name: 'Description' },
        { name: 'File', defaultValue: null },
        { name: 'TypeWork', defaultValue: null },
        { name: 'UsedInExport', defaultValue: 20 }
    ]
});