Ext.define('B4.model.dict.ArticleLawGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TomskArticleLawGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Description' },
        { name: 'Part' },
        { name: 'Article' },
        { name: 'PhysPersonPenalty' },
        { name: 'JurPersonPenalty' },
        { name: 'OffPersonPenalty' }
    ]
});