Ext.define('B4.model.protocolmhc.ArticleLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolMhcArticleLaw'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProtocolMhc', defaultValue: null },
        { name: 'ArticleLaw', defaultValue: null },
        { name: 'Description' }
    ]
});