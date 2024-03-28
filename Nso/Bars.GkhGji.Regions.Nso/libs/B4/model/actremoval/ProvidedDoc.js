Ext.define('B4.model.actremoval.ProvidedDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActRemovalProvidedDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActRemoval', defaultValue: null },
        { name: 'ProvidedDocGji', defaultValue: null },
        { name: 'DateProvided' }
    ]
});