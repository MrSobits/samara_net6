Ext.define('B4.model.diagnostic.DiagnosticResult', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DiagnosticResult'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Message' },
        { name: 'State' }
    ]
});