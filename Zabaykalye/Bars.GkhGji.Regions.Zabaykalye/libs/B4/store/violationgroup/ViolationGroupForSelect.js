Ext.define('B4.store.violationgroup.ViolationGroupForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.violationgroup.ViolationGroup'],
    autoLoad: false,
    storeId: 'violGroupForSelectStore',
    model: 'B4.model.violationgroup.ViolationGroup'
});