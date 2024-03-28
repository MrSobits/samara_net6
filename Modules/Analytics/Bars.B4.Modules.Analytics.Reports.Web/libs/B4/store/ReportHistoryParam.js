Ext.define('B4.store.ReportHistoryParam', {
    extend: 'B4.base.Store',
    idProperty: 'Name',
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'ReportHistory',
        listAction: 'ReportHistoryParamList'
    },
    fields: [
        { name: 'Name' },
        { name: 'Value' },
        { name: 'DisplayValue' },
        { name: 'DisplayName' }
    ]
});