Ext.define('B4.model.passport.OkiMunicipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OkiProviderPassport',
        listAction: 'MunicipalityForOki'
    },
    fields: [
        { name: 'Id', defaultValue: 0 },
        { name: 'Name', defaultValue: "" }
    ]
});