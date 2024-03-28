Ext.define('B4.model.objectcr.BuildContractBuilder', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuildContract',
        listAction: 'ListAvailableBuilders'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ContragentName' }
    ]
});