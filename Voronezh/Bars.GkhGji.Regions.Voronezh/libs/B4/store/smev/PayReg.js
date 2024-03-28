Ext.define('B4.store.smev.PayReg', {
    extend: 'B4.base.Store',
    requires: ['B4.model.smev.PayReg'],
    autoLoad: false,
    storeId: 'payRegStore',
    model: 'B4.model.smev.PayReg'
});