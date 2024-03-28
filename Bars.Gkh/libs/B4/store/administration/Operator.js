Ext.define('B4.store.administration.Operator', {
    extend: 'B4.base.Store',
    requires: ['B4.model.administration.Operator'],
    autoLoad: false,
    storeId: 'operatorStore',
    model: 'B4.model.administration.Operator'
});