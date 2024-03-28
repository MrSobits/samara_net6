Ext.define('B4.model.import.ChesImportFile', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ChesImportFile'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ChesImport' },
        { name: 'FileType' },
        { name: 'CheckState' },
        { name: 'IsImported' },
        { name: 'File' }
    ]
});