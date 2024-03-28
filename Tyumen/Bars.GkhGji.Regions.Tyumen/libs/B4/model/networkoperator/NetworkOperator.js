Ext.define('B4.model.networkoperator.NetworkOperator', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NetworkOperator'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'Description' },
        { name: 'Contragent', defaultValue: null }
    ]
});