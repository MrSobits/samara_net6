Ext.define('B4.model.specialobjectcr.BuildContractBuilder', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialBuildContract',
        listAction: 'ListAvailableBuilders'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ContragentName' }
    ]
});