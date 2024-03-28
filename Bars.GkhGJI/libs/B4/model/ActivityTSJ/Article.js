Ext.define('B4.model.activitytsj.Article', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActivityTsjArticle'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ArticleTsj', defaultValue: null },
        { name: 'ActivityTsjStatute', defaultValue: null },
        { name: 'ArticleTsjId', defaultValue: null },
        { name: 'IsNone', defaultValue: false },
        { name: 'TypeState', defaultValue: 10 },
        { name: 'Paragraph' },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Group' }
    ]
});