Ext.define('B4.model.diagnostic.CollectedDiagnosticResult', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CollectedDiagnosticResult'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ObjectCreateDate' },
        { name: 'State' }
    ]
});