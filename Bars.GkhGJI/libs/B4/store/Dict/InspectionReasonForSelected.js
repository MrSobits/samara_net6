Ext.define('B4.store.dict.InspectionReasonForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.InspectionReason'],
    autoLoad: false,
    storeId: 'inspectionReasonForSelectedStore',
    model: 'B4.model.dict.InspectionReason'
});