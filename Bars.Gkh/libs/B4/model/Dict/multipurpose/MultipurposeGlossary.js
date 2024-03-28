Ext.define('B4.model.dict.multipurpose.MultipurposeGlossary', {    
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'multipurposeglossary'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Code' },
        { name: 'Name' }
    ]
});