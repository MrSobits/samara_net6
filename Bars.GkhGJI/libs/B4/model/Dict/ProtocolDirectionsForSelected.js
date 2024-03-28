Ext.define('B4.model.dict.ProtocolDirectionsForSelected', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Code' }
    ]
});