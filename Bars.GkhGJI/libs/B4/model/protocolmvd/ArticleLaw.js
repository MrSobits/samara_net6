Ext.define('B4.model.protocolmvd.ArticleLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolMvdArticleLaw'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProtocolMvd', defaultValue: null },
        { name: 'ArticleLaw', defaultValue: null },
        { name: 'Description' }
    ]
});