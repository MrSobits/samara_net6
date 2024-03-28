Ext.define('B4.model.edolog.AppealCits', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'EdoIntegration'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'IsEdo' },
        { name: 'NumberGji' },
        { name: 'DateFrom' },
        { name: 'DateActual' },
        { name: 'Document' }
    ]
});