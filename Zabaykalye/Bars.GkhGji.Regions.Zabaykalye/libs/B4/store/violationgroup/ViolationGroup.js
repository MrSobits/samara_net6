Ext.define('B4.store.violationgroup.ViolationGroup', {
    extend: 'B4.base.Store',
    requires: ['B4.model.violationgroup.ViolationGroup'],
    autoLoad: false,
    storeId: 'violGroupStore',
    model: 'B4.model.violationgroup.ViolationGroup'
});