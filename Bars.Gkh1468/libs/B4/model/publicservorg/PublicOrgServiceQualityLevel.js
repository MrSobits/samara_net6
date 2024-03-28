Ext.define('B4.model.publicservorg.PublicOrgServiceQualityLevel', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublicOrgServiceQualityLevel'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name', defaultValue: null },
        { name: 'Value', defaultValue: null },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'ServiceOrg', defaultValue: null }
    ]
});