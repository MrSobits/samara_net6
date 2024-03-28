Ext.define('B4.store.dict.ZonalInspectionForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ZonalInspection'],
    autoLoad: false,
    storeId: 'zonalInspectionForSelectStore',
    model: 'B4.model.dict.ZonalInspection'
});