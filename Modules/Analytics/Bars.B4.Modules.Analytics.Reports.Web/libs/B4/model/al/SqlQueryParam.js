Ext.define('B4.model.al.SqlQueryParam', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'StoredReport',
        listAction: 'RunSqlQueryParameter',
        timeout: 1000*60*3
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});