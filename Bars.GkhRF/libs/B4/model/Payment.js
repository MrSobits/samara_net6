Ext.define('B4.model.Payment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Payment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Address' },
        { name: 'Municipality' },
        { name: 'ChargePopulationCr' },
        { name: 'PaidPopulationCr' },
        { name: 'ChargePopulationHireRf' },
        { name: 'PaidPopulationHireRf' },
        { name: 'ChargePopulationCr185' },
        { name: 'PaidPopulationCr185' },
        { name: 'ChargePopulationBldRepair' },
        { name: 'PaidPopulationBldRepair' }
    ]
});