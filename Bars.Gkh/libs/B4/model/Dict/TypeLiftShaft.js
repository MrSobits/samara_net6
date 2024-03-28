Ext.define('B4.model.dict.TypeLiftShaft', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'typeliftshaft'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});