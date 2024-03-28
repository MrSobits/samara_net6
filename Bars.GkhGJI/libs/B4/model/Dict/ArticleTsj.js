Ext.define('B4.model.dict.ArticleTsj', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ArticleTsj'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Group' },
        { name: 'Article' }
    ]
});