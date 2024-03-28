Ext.define('B4.model.BelayPolicy', {
    extend: 'B4.base.Model',
    requires: ['B4.enums.PolicyAction'],
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BelayPolicy'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocumentDate' },
        { name: 'DocumentStartDate' },
        { name: 'DocumentEndDate' },
        { name: 'DocumentNumber' },
        { name: 'LimitManOrgInsured' },
        { name: 'LimitManOrgHome' },
        { name: 'LimitCivilOne' },
        { name: 'LimitCivilInsured' },
        { name: 'LimitCivil' },
        { name: 'Cause' },
        { name: 'BelaySum' },
        { name: 'ContragentName' },
        { name: 'PolicyAction', defaultValue: 10 },
        { name: 'BelayOrganization', defaultValue: null },
        { name: 'BelayManOrgActivity', defaultValue: null },
        { name: 'BelayOrgKindActivity', defaultValue: null }
    ]
});