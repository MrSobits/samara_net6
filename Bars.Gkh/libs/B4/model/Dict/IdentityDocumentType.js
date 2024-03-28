Ext.define('B4.model.dict.IdentityDocumentType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'IdentityDocumentType'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'Regex' },
        { name: 'RegexErrorMessage' }
    ]
});