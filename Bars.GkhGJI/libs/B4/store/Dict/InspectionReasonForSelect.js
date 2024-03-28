Ext.define('B4.store.dict.InspectionReasonForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.InspectionReason'],
    autoLoad: false,
    storeId: 'inspectionReasonForSelectStore',
    model: 'B4.model.dict.InspectionReason'
});