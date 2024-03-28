Ext.define('B4.model.BasePropertyOwnerDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePropertyOwnerDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'PropertyOwnerProtocol' },
        { name: 'MonthlyPayment' },
        { name: 'ProtocolNumber' },
        { name: 'ProtocolDate' },
        { name: 'Decision' },
        { name: 'MethodFormFund' },
        { name: 'PropertyOwnerDecisionType' },
        { name: 'MoOrganizationForm' }
    ]
});