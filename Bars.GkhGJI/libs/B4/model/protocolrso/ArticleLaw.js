Ext.define('B4.model.protocolrso.ArticleLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolRSOArticleLaw'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ProtocolRSO', defaultValue: null },
        { name: 'ArticleLaw', defaultValue: null },
        { name: 'Description' }
    ]
});