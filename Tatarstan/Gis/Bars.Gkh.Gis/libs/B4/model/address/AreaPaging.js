Ext.define('B4.model.address.AreaPaging', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisAddress',
        listAction: 'AreaListPaging'
    },
    fields: [
        { name: 'Id' },
        { name: 'Name' }
    ]
});
