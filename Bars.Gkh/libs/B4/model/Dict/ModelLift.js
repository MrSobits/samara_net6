Ext.define('B4.model.dict.ModelLift', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'modellift'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});