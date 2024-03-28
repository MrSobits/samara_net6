Ext.define('B4.model.builder.Document', {
    extend: 'B4.base.Model',
    requires: ['B4.enums.YesNoNotSet',
            'B4.enums.TypeDocument'],
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuilderDocument'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Builder', defaultValue: null },
        { name: 'Period', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'Description' },
        { name: 'DocumentNum' },
        { name: 'DocumentName' },
        { name: 'DocumentDate',  type: 'date', dateFormat: 'Y-m-d\TH:i:s', useNull: true },
        { name: 'DocumentExist', defaultValue: 30 },
        { name: 'BuilderDocumentType', defaultValue: null },
        { name: 'File', defaultValue: null }
    ]
});