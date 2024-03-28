Ext.define('B4.model.address.PlacePaging', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisAddress',
        listAction: 'PlaceListPaging'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ]
});
