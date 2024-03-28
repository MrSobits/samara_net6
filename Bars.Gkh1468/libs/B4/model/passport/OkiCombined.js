Ext.define('B4.model.passport.OkiCombined', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OkiPassportCombined',
        listAction: 'GetList'
    },
    fields: [
        { name: 'PpNumber', defaultValue: "" },
        { name: 'Name', defaultValue: "" },
        { name: 'Value', defaultValue: "" },
        { name: 'InfoSupplier', defaultValue: "" },
        { name: 'IsMultiple', defaultValue: false }
    ]
});