Ext.define('B4.store.metavalueconstructor.DataFillerTree', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.metavalueconstructor.DataFillerTree'],
    autoLoad: false,
    model: 'B4.model.metavalueconstructor.DataFillerTree',
    defaultRootProperty: 'Children'
});