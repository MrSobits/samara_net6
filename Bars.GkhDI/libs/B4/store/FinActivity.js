Ext.define('B4.store.FinActivity', {
    extend: 'B4.base.Store',
    requires: ['B4.model.FinActivity'],
    autoLoad: false,
    storeId: 'finActivityStore',
    model: 'B4.model.FinActivity'
});