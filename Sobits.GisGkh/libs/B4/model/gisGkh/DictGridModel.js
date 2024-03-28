Ext.define('B4.model.gisGkh.DictGridModel', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NsiList'
    },
    fields: [
        { name: 'EntityName' },
        { name: 'ListGroup' },
        { name: 'GisGkhCode' },
        { name: 'GisGkhName' },
        { name: 'MatchDate' },
        { name: 'ModifiedDate' },
        { name: 'RefreshDate' }
    ]
});