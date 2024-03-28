Ext.define('B4.model.RegOpAccountDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegOpAccountDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'PropertyOwnerProtocol' },
        { name: 'RegOperator', defaultValue: null },
        { name: 'MonthlyPayment' },
        { name: 'PropertyOwnerDecisionType' },
        { name: 'MoOrganizationForm' },
        { name: 'MethodFormFund' }
    ]
});