Ext.define('B4.store.groups.GroupDiSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.groups.GroupDi'],
    autoLoad: false,
    storeId: 'groupsDiSelectedStore',
    model: 'B4.model.groups.GroupDi'
});