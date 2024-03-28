Ext.define('B4.model.dict.multipurpose.MultipurposeGlossaryItem', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'multipurposeglossaryitem'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Key' },
        { name: 'Value' },
        { name: 'Glossary' }
    ]
});