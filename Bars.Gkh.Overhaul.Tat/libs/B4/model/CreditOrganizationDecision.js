Ext.define('B4.model.CreditOrganizationDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CreditOrganizationDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'CreditOrganization', defaultValue: null },
        { name: 'SettlementAccount' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'PropertyOwnerDecisionType' },
        { name: 'MoOrganizationForm' },
        { name: 'PropertyOwnerProtocol' }
    ]
});