Ext.define('B4.store.decision.InspectionReason', {
    extend: 'B4.base.Store',
    requires: ['B4.model.decision.InspectionReason'],
    autoLoad: false,
    storeId: 'decisionInspectionReasonStore',
    model: 'B4.model.decision.InspectionReason'
});