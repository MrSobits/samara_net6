Ext.define('B4.store.TableLock', {
    extend: 'B4.base.Store',
    requires: ['B4.model.TableLock'],
    autoLoad: false,
    storeId: 'tableLockStore',
    model: 'B4.model.TableLock'
});