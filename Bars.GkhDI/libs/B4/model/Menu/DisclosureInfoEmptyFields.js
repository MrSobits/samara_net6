Ext.define('B4.model.menu.DisclosureInfoEmptyFields', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisclosureInfoEmptyFields'
    },
    fields: [
        { name: 'FieldName' },
        { name: 'PathId' }
    ]
});