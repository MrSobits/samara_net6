Ext.define('B4.store.dict.ZonalInspectionForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ZonalInspection'],
    autoLoad: false,
    storeId: 'zonalInspectionForSelectedStore',
    model: 'B4.model.dict.ZonalInspection'
});