Ext.define('B4.model.finactivity.DocByYear', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinActivityDocByYear'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'TypeDocByYearDi', defaultValue: 10 },
        { name: 'Year' },
        { name: 'DocumentDate' }
        
    ]
});