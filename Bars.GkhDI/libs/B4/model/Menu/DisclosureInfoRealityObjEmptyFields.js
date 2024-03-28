Ext.define('B4.model.menu.DisclosureInfoRealityObjEmptyFields', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisclosureInfoRealityObjEmptyFields'
    },
    fields: [
        { name: 'FieldName' },
        { name: 'PathId' },
        { name: 'Address' }
    ]
});