Ext.define('B4.store.TypeWorksForSelect', {
    extend: 'B4.base.Store',
    fields: ['Id', 'Name', 'Address'],
    autoLoad: false,
    storeId: 'typeWorkForSelect',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CostLimit',
        listAction: 'TypeWorksForSelect'
    }
});