Ext.define('B4.model.specialobjectcr.ContractCrTypeWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialContractCrTypeWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ContractCr' },
        { name: 'TypeWork' },
        { name: 'Sum' }
    ]
});