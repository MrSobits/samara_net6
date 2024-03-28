Ext.define('B4.model.actcheck.ProvidedDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckProvidedDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheck', defaultValue: null },
        { name: 'ProvidedDocGji', defaultValue: null },
        { name: 'DateProvided' }
    ]
});