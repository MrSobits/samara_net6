Ext.define('B4.model.realityobj.BelayPolicy', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.PolicyAction'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectBelayPolicy'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BelayOrganizationName' },
        { name: 'ManagingOrganizationName' },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'DocumentStartDate' },
        { name: 'DocumentEndDate' },
        { name: 'LimitManOrgInsured' },
        { name: 'LimitCivilOne' },
        { name: 'Cause' },
        { name: 'PolicyAction', defaultValue: 10 }
    ]
});