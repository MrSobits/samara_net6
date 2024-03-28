Ext.define('B4.store.administration.operator.Contragent', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Contragent'],
    autoLoad: false,
    storeId: 'operatorContragent',
    model: 'B4.model.Contragent',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Operator',
        listAction: 'ListContragent'
    }
});