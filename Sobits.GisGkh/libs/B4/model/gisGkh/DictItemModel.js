Ext.define('B4.model.gisGkh.DictItemModel', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NsiItem',
        timeout: 60000
    },
    fields: [
        { name: 'EntityItemId' },
        { name: 'GisGkhGUID' },
        { name: 'GisGkhItemCode' },
        { name: 'NsiList' },
        { name: 'IsActual' },
        { name: 'Name' }
    ]
});