Ext.define('B4.model.PrevAccumulatedAmountDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PrevAccumulatedAmountDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'AccumulatedAmount' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'PropertyOwnerDecisionType' },
        { name: 'MoOrganizationForm' },
        { name: 'PropertyOwnerProtocol' }
    ]
});