Ext.define('B4.model.metavalueconstructor.DataMetaInfoTree', {
    extend: 'B4.model.metavalueconstructor.DataMetaInfo',
    idProperty: 'Id',
    proxy: {
        type: 'memory'
    },
    fields: [
        { name: 'iconCls', defaultValue: '' },
        { name: 'expanded', defaultValue: true },
        { name: 'Children' }
    ]
});