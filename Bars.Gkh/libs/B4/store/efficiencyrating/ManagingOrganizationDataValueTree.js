Ext.define('B4.store.efficiencyrating.ManagingOrganizationDataValueTree', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.efficiencyrating.ManagingOrganizationDataValueTree'],
    autoLoad: false,
    model: 'B4.model.efficiencyrating.ManagingOrganizationDataValueTree',
    defaultRootProperty: 'Children'
});