Ext.define('B4.model.dict.Official', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Official'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Operator', defaultValue: null },
        { name: 'Fio' },
        { name: 'Code' }
    ]
});