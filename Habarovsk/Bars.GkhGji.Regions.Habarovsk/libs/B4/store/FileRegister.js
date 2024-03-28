Ext.define('B4.store.FileRegister', {
    extend: 'B4.base.Store',
    autoLoad: false,
    requires: ['B4.model.FileRegister'],
    storeId: 'fileRegisterStore',
    model: 'B4.model.FileRegister'
});