Ext.define('B4.model.passport.HouseCombined', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HousePassportCombined',
        listAction: 'GetList'
    },
    fields: [
        { name: 'PpNumber', defaultValue: "" },
        { name: 'Name', defaultValue: "" },
        { name: 'Value', defaultValue: "" },
        { name: 'InfoSupplier', defaultValue: "" },
        { name: 'IsMultiple', defaultValue: false}
    ]
});