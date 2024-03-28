Ext.define('B4.store.BaseJurPerson', {
    extend: 'B4.base.Store',
    requires: ['B4.model.BaseJurPerson'],
    autoLoad: false,
    storeId: 'baseJurPersonStore',
    model: 'B4.model.BaseJurPerson'
});