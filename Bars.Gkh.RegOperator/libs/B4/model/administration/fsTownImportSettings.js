Ext.define('B4.model.administration.fsTownImportSettings', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FsGorodImportInfo'
    },
    fields: [
        { name: 'Id' },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'DataHeadIndex' },
        { name: 'Delimiter' }
    ]
});