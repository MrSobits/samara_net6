Ext.define('B4.model.constructionobject.Photo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ConstructionObjectPhoto',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ConstructionObject', defaultValue: null },
        { name: 'Date' },
        { name: 'Name' },
        { name: 'Group', defaultValue: 10 },
        { name: 'File', defaultValue: null },
        { name: 'Description' }
    ]
});