Ext.define('B4.model.metavalueconstructor.DataFillerTree', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'memory'
    },
    fields: [
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Children'},
        { name: 'iconCls', defaultValue: '' },
        { name: 'expanded', defaultValue: true }
    ]
});