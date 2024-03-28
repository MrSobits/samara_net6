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
        { name: 'PropertyOwnerProtocol' },
        { name: 'SizeOfPaymentSubject' },
        { name: 'SizeOfPaymentOwners' },
        { name: 'MoOrganizationForm' },
        { name: 'PaymentDateStart' },
        { name: 'PaymentDateEnd' }
    ]
});