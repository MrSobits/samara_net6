Ext.define('B4.store.dict.InspectorForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Inspector'],
    autoLoad: false,
    storeId: 'inspectorForSelectStore',
    model: 'B4.model.dict.Inspector'
});