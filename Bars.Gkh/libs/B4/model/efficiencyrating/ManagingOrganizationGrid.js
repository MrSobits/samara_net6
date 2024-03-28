Ext.define('B4.model.efficiencyrating.ManagingOrganizationGrid', {
    extend: 'B4.model.efficiencyrating.ManagingOrganization',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManagingOrganizationEfficiencyRating'
    },
    fields: [
        { name: 'ObjectId', useNull: true },
        { name: 'Municipality' },
        { name: 'Inn' },
        { name: 'Kpp' },
        { name: 'Ogrn' }
    ]
});