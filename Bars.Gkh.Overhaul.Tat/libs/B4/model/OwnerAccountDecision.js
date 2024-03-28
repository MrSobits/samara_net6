Ext.define('B4.model.OwnerAccountDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OwnerAccountDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'OwnerAccountType' },
        { name: 'Contragent', defaultValue: null },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'PropertyOwnerDecisionType' },
        { name: 'MoOrganizationForm' },
        { name: 'PropertyOwnerProtocol' }
    ]
});