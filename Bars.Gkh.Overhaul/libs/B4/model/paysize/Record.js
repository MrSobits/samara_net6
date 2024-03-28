Ext.define('B4.model.paysize.Record', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaysizeRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Paysize', defaultValue: null },
        { name: 'Municipality', defaultValue: null },
        { name: 'Value', defaultValue: null },
        { name: 'Name' },
        { name: 'text' }
    ]
});