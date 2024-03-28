Ext.define('B4.model.protocolgji.ArticleLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProtocolArticleLaw'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Protocol', defaultValue: null },
        { name: 'ArticleLaw', defaultValue: null },
        { name: 'Description' }
    ]
});