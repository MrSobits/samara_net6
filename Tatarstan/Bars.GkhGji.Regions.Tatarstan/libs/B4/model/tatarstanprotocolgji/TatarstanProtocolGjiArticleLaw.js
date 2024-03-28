Ext.define('B4.model.tatarstanprotocolgji.TatarstanProtocolGjiArticleLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TatarstanProtocolGjiArticleLaw'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ArticleLaw' },
        { name: 'Description' }
    ]
});