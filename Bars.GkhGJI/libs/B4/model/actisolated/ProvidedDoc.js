Ext.define('B4.model.actisolated.ProvidedDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActIsolatedProvidedDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActIsolated', defaultValue: null },
        { name: 'ProvidedDocGji', defaultValue: null },
        { name: 'DateProvided' }
    ]
});