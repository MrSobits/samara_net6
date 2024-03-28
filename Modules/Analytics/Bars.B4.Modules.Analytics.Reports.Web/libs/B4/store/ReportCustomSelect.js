Ext.define('B4.store.ReportCustomSelect', {
    extend: 'B4.base.Store',
    fields: ['Key', 'Name'],
    autoLoad: false,
    proxy: {
        type: 'b4proxy',
        controllerName: 'CodedReport',
        listAction: 'List'
    }
});
