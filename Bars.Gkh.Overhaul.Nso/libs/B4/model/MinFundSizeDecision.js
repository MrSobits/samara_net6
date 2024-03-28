Ext.define('B4.model.MinFundSizeDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MinFundSizeDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'SubjectMinFundSize' },
        { name: 'OwnerMinFundSize' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'PropertyOwnerDecisionType' },
        { name: 'MoOrganizationForm' },
        { name: 'PropertyOwnerProtocol' }
    ]
});