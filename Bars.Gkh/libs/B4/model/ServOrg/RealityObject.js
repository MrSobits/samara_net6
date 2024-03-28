Ext.define('B4.model.servorg.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ServiceOrgRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ServiceOrganization', defaultValue: null },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Address'},
        { name: 'Municipality' }

    ]
});