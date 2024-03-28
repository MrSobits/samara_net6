Ext.define('B4.store.dict.RoleForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Role'],
    autoLoad: false,
    storeId: 'roleForSelectedStore',
    model: 'B4.model.Role'
});