Ext.define('B4.model.Paysize', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Paysize',
        timeout: 120000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DateStart' },
        { name: 'DateEnd', defaultValue: null },
        { name: 'Indicator', defaultValue: 20 },
        { name: 'HasCharges', defaultValue: false }
    ]
});