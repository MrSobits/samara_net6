Ext.define('B4.model.dict.KindCheckGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'KindCheckGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name', defaultValue: null },
        { name: 'Code', defaultValue: 1 },
        { name: 'Description', defaultValue: null }
    ]
});