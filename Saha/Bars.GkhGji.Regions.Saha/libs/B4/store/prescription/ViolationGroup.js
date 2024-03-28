Ext.define('B4.store.prescription.ViolationGroup', {
    extend: 'B4.base.Store',
    requires: ['B4.model.violationgroup.ViolationGroup'],
    autoLoad: false,
    storeId: 'prescriptionViolGroupStore',
    model: 'B4.model.violationgroup.ViolationGroup'
});