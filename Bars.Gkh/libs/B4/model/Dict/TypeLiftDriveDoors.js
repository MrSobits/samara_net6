Ext.define('B4.model.dict.TypeLiftDriveDoors', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'typeliftdrivedoors'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});