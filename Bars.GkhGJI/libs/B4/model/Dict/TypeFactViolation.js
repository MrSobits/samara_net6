Ext.define('B4.model.dict.TypeFactViolation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeFactViolation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' }
    ]
});