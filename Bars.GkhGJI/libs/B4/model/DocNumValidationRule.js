Ext.define('B4.model.DocNumValidationRule', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'DocNumValidationRule'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RuleId' },
        { name: 'Name' },
        { name: 'TypeDocumentGji', defaultValue: 10 }
    ]
});