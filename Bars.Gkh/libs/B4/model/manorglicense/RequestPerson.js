Ext.define('B4.model.manorglicense.RequestPerson', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgRequestPerson'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'LicRequest' },
        { name: 'Person' },
        { name: 'PersonFullName' },
        { name: 'Position' }
    ]
});