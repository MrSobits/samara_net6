Ext.define('B4.store.actremoval.ViolationGroup', {
    extend: 'B4.base.Store',
    requires: ['B4.model.violationgroup.ViolationGroup'],
    autoLoad: false,
    storeId: 'actremovalViolGroupStore',
    model: 'B4.model.violationgroup.ViolationGroup'
});