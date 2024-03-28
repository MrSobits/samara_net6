Ext.define('B4.model.disposal.KnmReason', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'KnmReason'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Decision', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'ErknmTypeDocument' },
        { name: 'Description' },
        { name: 'DocumentType' }
    ]
});