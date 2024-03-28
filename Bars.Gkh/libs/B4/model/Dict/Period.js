Ext.define('B4.model.dict.Period', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Period'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});