Ext.define('B4.model.protocol197.ArticleLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Protocol197ArticleLaw'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Protocol197', defaultValue: null },
        { name: 'ArticleLaw', defaultValue: null },
        { name: 'Description' }
    ]
});