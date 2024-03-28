Ext.define('B4.model.dpkrdocument.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DpkrDocumentRealityObject'
    },
    fields: [
        { name: 'Municipality' },
        { name: 'Address' }
    ]
});