Ext.define('B4.store.networkoperator.NetworkOperator', {
    extend: 'B4.base.Store',
    requires: ['B4.model.networkoperator.NetworkOperator'],
    autoLoad: false,
    storeId: 'networkOperatorStore',
    model: 'B4.model.networkoperator.NetworkOperator'
});