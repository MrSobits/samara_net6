Ext.define('B4.model.realityobj.ResOrg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectResOrg'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'ResourceOrg', defaultValue: null },
        { name: 'ResourceOrgId' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});