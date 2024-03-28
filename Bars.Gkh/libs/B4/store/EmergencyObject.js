Ext.define('B4.store.EmergencyObject', {
    extend: 'B4.base.Store',
    requires: ['B4.model.EmergencyObject'],
    autoLoad: false,
    storeId: 'emergencyObjStore',
    model: 'B4.model.EmergencyObject'
});