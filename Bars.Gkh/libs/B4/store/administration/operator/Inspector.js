Ext.define('B4.store.administration.operator.Inspector', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Inspector'],
    autoLoad: false,
    storeId: 'operatorInspector',
    model: 'B4.model.dict.Inspector',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Operator',
        listAction: 'ListInspector'
    }
});