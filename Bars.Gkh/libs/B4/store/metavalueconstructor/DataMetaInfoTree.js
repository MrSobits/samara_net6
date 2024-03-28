Ext.define('B4.store.metavalueconstructor.DataMetaInfoTree', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.metavalueconstructor.DataMetaInfoTree'],
    autoLoad: false,
    model: 'B4.model.metavalueconstructor.DataMetaInfoTree',
    defaultRootProperty: 'Children'
});