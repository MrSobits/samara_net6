Ext.define('B4.model.decision.ProvidedDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionProvidedDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Decision', defaultValue: null },
        { name: 'ProvidedDocGji', defaultValue: null },
        { name: 'Code' },
        { name: 'Description' }
    ]
});