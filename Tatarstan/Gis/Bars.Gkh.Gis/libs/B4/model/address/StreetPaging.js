Ext.define('B4.model.address.StreetPaging', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisAddress',
        listAction: 'StreetListPaging'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'ShortName' },
        { name: 'FormalName' }
    ]
});
