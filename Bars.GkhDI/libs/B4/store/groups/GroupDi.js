Ext.define('B4.store.groups.GroupDi', {
    extend: 'B4.base.Store',
    requires: ['B4.model.groups.GroupDi'],
    autoLoad: false,
    storeId: 'groupsDiStore',
    model: 'B4.model.groups.GroupDi'
});