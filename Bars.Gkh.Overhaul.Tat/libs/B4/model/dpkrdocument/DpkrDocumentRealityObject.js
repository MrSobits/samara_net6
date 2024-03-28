Ext.define('B4.model.dpkrdocument.DpkrDocumentRealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DpkrDocumentRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address' },
        { name: 'Municipality' }
    ]
});