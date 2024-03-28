Ext.define('B4.model.specialobjectcr.BuildContractTypeWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialBuildContractTypeWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BuildContract' },
        { name: 'TypeWork' },
        { name: 'Sum' }
    ]
});