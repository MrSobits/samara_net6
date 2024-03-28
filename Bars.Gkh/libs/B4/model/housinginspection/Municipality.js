Ext.define('B4.model.housinginspection.Municipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HousingInspectionMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'HousingInspection', defaultValue: null },
        { name: 'Municipality', defaultValue: null }
    ]
});