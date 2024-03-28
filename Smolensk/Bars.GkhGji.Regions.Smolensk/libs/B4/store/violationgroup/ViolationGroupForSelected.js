Ext.define('B4.store.violationgroup.ViolationGroupForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.violationgroup.ViolationGroup'],
    autoLoad: false,
    storeId: 'violGroupForSelectedStore',
    model: 'B4.model.violationgroup.ViolationGroup'
});