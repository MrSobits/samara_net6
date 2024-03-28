Ext.define('B4.store.protocolgji.ViolationGroup', {
    extend: 'B4.base.Store',
    requires: ['B4.model.violationgroup.ViolationGroup'],
    autoLoad: false,
    storeId: 'actcheckViolGroupStore',
    model: 'B4.model.violationgroup.ViolationGroup'
});