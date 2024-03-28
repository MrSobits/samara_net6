Ext.define('B4.model.dict.TypeLift', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'typelift'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});