Ext.define('B4.model.dict.StateByType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ObjectCr',
        listAction: 'GetTypeStates'
    },    
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ]
});