Ext.define('B4.model.appealcits.ObjectCrList', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppCitPrFondOperations',
        listAction: 'GetListObjectCr'
    },
    fields: [
        { name: 'Id' },
        { name: 'Program' },
        { name: 'Address' }
    ]
});