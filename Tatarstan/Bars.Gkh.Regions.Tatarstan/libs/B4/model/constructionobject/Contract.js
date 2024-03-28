Ext.define('B4.model.constructionobject.Contract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'constructionobjectcontract',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ConstructionObject', defaultValue: null },
        { name: 'State', defaultValue: null },
        { name: 'Type', defaultValue: 10 },
        { name: 'Name' },
        { name: 'Date' },
        { name: 'Number' },
        { name: 'File', defaultValue: null },
        { name: 'Sum', defaultValue: null },
        { name: 'DateStart', defaultValue: null },
        { name: 'DateEnd', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'DateStartWork', defaultValue: null },
        { name: 'DateEndWork', defaultValue: null }
    ]
});