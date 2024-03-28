Ext.define('B4.model.warningdoc.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WarningDocAnnex'
    },
    fields: [
        { name: 'Id' },
        { name: 'WarningDoc' },
        { name: 'Name' },
        { name: 'DocumentDate' },
        { name: 'Description' },
        { name: 'File' },
    ]
});