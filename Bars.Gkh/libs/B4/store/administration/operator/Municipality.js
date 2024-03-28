Ext.define('B4.store.administration.operator.Municipality', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Municipality'],
    autoLoad: false,
    storeId: 'operatorMunicipality',
    model: 'B4.model.dict.Municipality',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Operator',
        listAction: 'ListMunicipality'
    }
});