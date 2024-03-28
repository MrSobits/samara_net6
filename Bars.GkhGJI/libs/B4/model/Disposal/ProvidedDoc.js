Ext.define('B4.model.disposal.ProvidedDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalProvidedDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Disposal', defaultValue: null },
        { name: 'ProvidedDocGji', defaultValue: null },
        { name: 'Code' },
        { name: 'Description' }
    ]
});