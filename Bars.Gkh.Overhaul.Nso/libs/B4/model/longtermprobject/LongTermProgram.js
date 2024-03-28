Ext.define('B4.model.longtermprobject.LongTermProgram', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LongTermProgram',
        listAction: 'List'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CommonEstateObject' },
        { name: 'Year' },
        { name: 'Sum' }
    ]
});