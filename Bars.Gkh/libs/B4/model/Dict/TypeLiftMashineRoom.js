Ext.define('B4.model.dict.TypeLiftMashineRoom', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'typeliftmashineroom'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});