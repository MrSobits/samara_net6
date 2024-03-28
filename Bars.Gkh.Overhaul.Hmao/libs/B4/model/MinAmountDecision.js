Ext.define('B4.model.MinAmountDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MinAmountDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'PropertyOwnerDecisionType' },
        { name: 'MoOrganizationForm' },
        { name: 'PropertyOwnerProtocol' },
        { name: 'SizeOfPaymentOwners' },
        { name: 'SizeOfPaymentSubject' },
        { name: 'PaymentDateStart' },
        { name: 'PaymentDateEnd' }
    ]
});