Ext.define('B4.model.BelayManOrgActivity', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BelayManOrgActivity'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ContragentName' },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'ContragentInn' },
        { name: 'Municipality' }
    ]
});