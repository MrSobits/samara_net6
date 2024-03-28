Ext.define('B4.model.dict.ArticleLawGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ArticleLawGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Description' },
        { name: 'Part' },
        { name: 'KBK' },
        { name: 'Article' },
        { name: 'GisGkhCode' },
        { name: 'GisGkhGuid' }
    ]
});