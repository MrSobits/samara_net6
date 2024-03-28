Ext.define('B4.model.SaldoRefresh', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SaldoRefresh'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Group' }
    ]
});