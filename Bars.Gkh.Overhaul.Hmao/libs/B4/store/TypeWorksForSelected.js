Ext.define('B4.store.TypeWorksForSelected', {
    extend: 'B4.base.Store',
    fields: ['Id', 'Name', 'Address'],
    autoLoad: false,
    storeId: 'typeWorkForSelected',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CostLimit',
        listAction: 'TypeWorksForSelect'
    }
});