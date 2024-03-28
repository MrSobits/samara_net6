Ext.define('B4.model.dict.NormativeDocItem', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NormativeDocItem'
    },
    fields: [
        { name: 'Id' },
        { name: 'Number' },
        { name: 'Text' },
        { name: 'NormativeDoc', 'defaultValue': null },
        { name: 'NormativeDocName' },
        { name: 'NormativeDocId' }
    ]
});