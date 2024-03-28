Ext.define('B4.store.actcheck.ViolationGroup', {
    extend: 'B4.base.Store',
    requires: ['B4.model.violationgroup.ViolationGroup'],
    autoLoad: false,
    storeId: 'actcheckViolGroupStore',
    model: 'B4.model.violationgroup.ViolationGroup'
});