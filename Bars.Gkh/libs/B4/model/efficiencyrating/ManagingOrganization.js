Ext.define('B4.model.efficiencyrating.ManagingOrganization', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManagingOrganizationEfficiencyRating'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrganization' },
        { name: 'Period' },
        { name: 'Rating' },
        { name: 'Dynamics' }
    ]
});