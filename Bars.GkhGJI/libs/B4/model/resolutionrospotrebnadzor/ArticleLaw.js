Ext.define('B4.model.resolutionrospotrebnadzor.ArticleLaw', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResolutionRospotrebnadzorArticleLaw'
    },
    fields: [
        { name: 'Id' },
        { name: 'Resolution', useNull: false },
        { name: 'Name' },
        { name: 'Description' }
    ]
});