Ext.define('B4.model.dict.ZonalInspectionMunicipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ZonalInspectionMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' }
    ]
});