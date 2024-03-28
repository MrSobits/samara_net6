Ext.define('B4.model.integrations.nsi.OrganizationWork', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Nsi',
        listAction: 'GetOrganizationWorks'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'MeasureName' },
        { name: 'Description' }
    ]
});