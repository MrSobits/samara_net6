Ext.define('B4.model.dict.MeteringDevice', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.TypeAccounting'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'meteringdevice'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'AccuracyClass' },
        { name: 'TypeAccounting', defaultValue: 10 }
    ]
});