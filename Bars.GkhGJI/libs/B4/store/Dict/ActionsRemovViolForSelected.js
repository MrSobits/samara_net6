Ext.define('B4.store.dict.ActionsRemovViolForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ActionsRemovViol'],
    autoLoad: false,
    storeId: 'actionsRemovViolForSelectedStore',
    model: 'B4.model.dict.ActionsRemovViol'
});