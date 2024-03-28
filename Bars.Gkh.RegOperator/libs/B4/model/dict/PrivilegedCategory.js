Ext.define('B4.model.dict.PrivilegedCategory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PrivilegedCategory'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'Percent' },
        { name: 'DateFrom', defaultValue: null },
        { name: 'DateTo' },
        { name: 'LimitArea' }
    ]
});