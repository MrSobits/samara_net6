Ext.define('B4.model.resolpros.ArticleLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolProsArticleLaw'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ResolPros', defaultValue: null },
        { name: 'ArticleLaw', defaultValue: null },
        { name: 'Description' }
    ]
});