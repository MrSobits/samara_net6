Ext.define('B4.model.SpecialAccountDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialAccountDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'PropertyOwnerProtocol' },
        { name: 'TypeOrganization', defultValue: 10 },
        { name: 'RegOperator', defaultValue: null },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'MinimalFundVolume' },
        { name: 'SpecialAccount' },
        { name: 'MonthlyPayment' },
        { name: 'AccountNumber' },
        { name: 'OpenDate' },
        { name: 'CloseDate' },
        { name: 'BankHelpFile' },
        { name: 'CreditOrg' },
        { name: 'PropertyOwnerDecisionType' },
        { name: 'MoOrganizationForm' },
        { name: 'MethodFormFund' }
    ]
});